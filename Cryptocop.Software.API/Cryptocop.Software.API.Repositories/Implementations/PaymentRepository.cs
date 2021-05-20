using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public PaymentRepository(CryptocopDbContext dbContext, IUserRepository userRepository = null)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            var user = _userRepository.findUserByEmail(email);
            if(user == null){ return; }
            var entity = new PaymentCard
            {
                UserId = user.Id,
                CardholderName = paymentCard.CardholderName,
                CardNumber = paymentCard.CardNumber,
                Month = paymentCard.Month,
                Year = paymentCard.Year,
            };
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return;
        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
            var user = _userRepository.findUserByEmail(email);
            if(user == null){ return null; }

            return _dbContext.PaymentCards
                    .Where(p => p.UserId == user.Id)
                    .Select(p => 
                        new PaymentCardDto
                        {
                            Id = p.Id,
                            CardholderName = p.CardholderName,
                            CardNumber = p.CardNumber,
                            Month = p.Month,
                            Year = p.Year,
                        } 
                    ).ToList();
        }

        public PaymentCardDto PaymentCardById(int cardId)
        {
            return _dbContext.PaymentCards.Select(p => 
                new PaymentCardDto
                {
                    Id = p.Id,
                    CardholderName = p.CardholderName,
                    CardNumber = p.CardNumber,
                    Month = p.Month,
                    Year = p.Year,
                }).FirstOrDefault(p => p.Id == cardId);
        }
    }
}