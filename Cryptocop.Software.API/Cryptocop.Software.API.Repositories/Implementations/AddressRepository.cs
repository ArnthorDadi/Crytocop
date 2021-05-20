using System;
using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly IUserRepository _userRepository;


        public AddressRepository(CryptocopDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public void AddAddress(string email, AddressInputModel address)
        {
            var user = _userRepository.findUserByEmail(email);
            if(user == null){ return; }
            var entity = new Address
            {
                UserId = user.Id,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,   
            };
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return;
        }

        public IEnumerable<AddressDto> GetAllAddresses(string email)
        {
            var user = _userRepository.findUserByEmail(email);
            if(user == null){ return null; }
            return _dbContext.Addresses
                .Where(a => a.UserId == user.Id)
                .Select(a => 
                    new AddressDto
                    {
                        Id = a.Id,
                        StreetName = a.StreetName,
                        HouseNumber = a.HouseNumber,
                        ZipCode = a.ZipCode,
                        Country = a.Country,
                        City = a.City,
                    }
                ).ToList();
        }

        public void DeleteAddress(string email, int addressId)
        {
            var user = _userRepository.findUserByEmail(email);
            if(user == null){ return; }
            var addressToDelete = _dbContext.Addresses.FirstOrDefault(a => 
                                            a.Id == addressId 
                                            && a.UserId == user.Id);
            if(addressToDelete == null){ return; }
            _dbContext.Addresses.Remove(addressToDelete);
            _dbContext.SaveChanges();
            return;
        }

        public AddressDto AddressById(int addressId)
        {
            return _dbContext.Addresses.Select(a =>
                new AddressDto
                {
                    Id = a.Id,
                    StreetName = a.StreetName,
                    HouseNumber = a.HouseNumber,
                    ZipCode = a.ZipCode,
                    Country = a.Country,
                    City = a.City,
                }
            ).FirstOrDefault(a => a.Id == addressId);
        }
    }
}