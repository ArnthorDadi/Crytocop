using System;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly ITokenRepository _tokenRepository;
        public UserRepository(CryptocopDbContext dbContext, ITokenRepository tokenRepository = null)
        {
            _dbContext = dbContext;
            _tokenRepository = tokenRepository;
        }

        public UserDto CreateUser(RegisterInputModel registerInputModel)
        {
            // Check if Email taken
            var user = _dbContext.Users
                                    .Where(u => u.Email == registerInputModel.Email)
                                    .FirstOrDefault();
            if(user != null){ return null; /*throw new Exception("Email Taken");*/ }

            // Create User
            string fullName = registerInputModel.FullName;
            string email = registerInputModel.Email;
            string password = registerInputModel.Password;
            var entity = createUser(fullName, email, password);

            // Add User
            _dbContext.Add(entity);
            
            // Create Token
            var token = _tokenRepository.CreateNewToken();
            
            _dbContext.SaveChanges(); // Commit

            string hashedPassword = HashingHelper.HashPassword(password);
            // need the Id so lets query it
            return createUserDto(email, hashedPassword, token.Id);
        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            // Check if user exists
            string hashedPassword = HashingHelper.HashPassword(loginInputModel.Password);

            var user = _dbContext.Users.FirstOrDefault(u => 
                                                    u.Email == loginInputModel.Email
                                                    && u.HashedPassword == hashedPassword
                                                    );

            if(user == null){ return null;/*throw new Exception("User not Found");*/ }

            // Create Token
            var token = _tokenRepository.CreateNewToken();

            string email = loginInputModel.Email;
            return createUserDto(email, hashedPassword, token.Id);
        }

        private User createUser(string FullName, string Email, string Password)
        {
            return new User
            {
                FullName = FullName,
                Email = Email,
                HashedPassword = HashingHelper.HashPassword(Password),
            };
        }

        private UserDto createUserDto(string Email, string hashedPassword, int tokenId)
        {
            var user = _dbContext.Users
                        .Where(u => 
                            u.Email == Email &&
                            u.HashedPassword == hashedPassword)
                        .Select(u => new UserDto{
                            Id = u.Id,
                            FullName = u.FullName,
                            Email = u.Email
                        }).FirstOrDefault();
            user.TokenId = tokenId;
            return user;
        }

        public UserDto findUserByEmail(string Email)
        {
            var user = _dbContext.Users
                        .Where(u => u.Email == Email)
                        .Select(u => new UserDto{
                            Id = u.Id,
                            FullName = u.FullName,
                            Email = u.Email
                        }).FirstOrDefault();
            user.TokenId = -1;
            return user;
        }
    }
}