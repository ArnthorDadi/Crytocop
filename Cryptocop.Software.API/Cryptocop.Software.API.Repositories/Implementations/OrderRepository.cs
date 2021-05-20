using System;
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
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderRepository(CryptocopDbContext dbContext, IPaymentRepository paymentRepository, IUserRepository userRepository, IAddressRepository addressRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _dbContext = dbContext;
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            //return findtOrders(email);
            var orders = _dbContext.Orders
                        .Where(o => o.Email == email)
                        .Select(o => 
                            new OrderDto
                            {
                                Id = o.Id,
                                Email = o.Email,
                                FullName = o.FullName,
                                StreetName = o.StreetName,
                                HouseNumber = o.HouseNumber,
                                ZipCode = o.ZipCode,
                                Country = o.Country,
                                City = o.City,
                                CardholderName = o.CardHolderName,
                                CreditCard = o.MaskedCreditCard,
                                OrderDate = o.OrderDate.ToString("dd/MM/yyyy").Replace("- ", ".").Replace(" ", ""), // • Represented as 01.01.2020
                                TotalPrice = o.TotalPrice//,
                                /*OrderItems = _dbContext.OrderItems
                                                .Where(oi => oi.OrderId == o.Id)
                                                .Select(oi =>
                                                    new OrderItemDto
                                                    {
                                                        Id = oi.Id,
                                                        ProductIdentifier = oi.ProductIdentifier,
                                                        Quantity = oi.Quantity,
                                                        UnitPrice = oi.UnitPrice,
                                                        TotalPrice = oi.TotalPrice
                                                    }
                                                ).ToList()*/
                            }
                        ).ToList();
            foreach(var order in orders)
            {
                order.OrderItems = _dbContext.OrderItems
                                    .Where(oi => oi.OrderId == order.Id)
                                    .Select(oi =>
                                        new OrderItemDto
                                        {
                                            Id = oi.Id,
                                            ProductIdentifier = oi.ProductIdentifier,
                                            Quantity = oi.Quantity,
                                            UnitPrice = oi.UnitPrice,
                                            TotalPrice = oi.TotalPrice
                                        }
                                    ).ToList();
            }
            return orders;
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            // Need user
            var user = _userRepository.findUserByEmail(email);
            // Need address
            var addresses = _addressRepository.GetAllAddresses(email);
            // Need PaymentCard
            var payment_cards = _paymentRepository.GetStoredPaymentCards(email);
            
            // Check if all resources exists
            if(user == null){ return null; }
            if(addresses == null){ return null; }
            if(payment_cards == null ){ return null; }

            AddressDto address = null;
            foreach(var currAddress in addresses){
                if(currAddress.Id == order.AddressId)
                { 
                    address = currAddress;  
                    break;
                }
            }
            if(address == null){ return null; }

            PaymentCardDto payment_card = null;
            foreach(var currPayment_card in payment_cards){
                if(currPayment_card.Id == order.PaymentCardId)
                { 
                    payment_card = currPayment_card;  
                    break;
                }
            }
            if(payment_card == null){ return null; }


            var orderItems = _shoppingCartRepository.GetCartItems(email);
            float totalPrice = 0.0F;

            foreach(var orderItem in orderItems)
            {
                totalPrice += orderItem.TotalPrice;
            }

            var entity = new Order
            {
                Email = user.Email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardHolderName = payment_card.CardholderName,
                MaskedCreditCard = PaymentCardHelper.MaskPaymentCard(payment_card.CardNumber),
                OrderDate =  DateTime.Now,
                TotalPrice = totalPrice,
            };
            _dbContext.Add(entity);
            _dbContext.SaveChanges();

            List<OrderItem> orderItemsEntity = new List<OrderItem>();

            foreach(var orderItem in orderItems)
            {
                orderItemsEntity.Add(new OrderItem
                {
                    Id = orderItem.Id,
                    OrderId = entity.Id,
                    ProductIdentifier = orderItem.ProductIdentifier,
                    Quantity = (int) orderItem.Quantity,
                    UnitPrice = (int) orderItem.UnitPrice,
                    TotalPrice = (int) orderItem.Quantity * (int) orderItem.UnitPrice
                });
            }
            _dbContext.AddRange(orderItemsEntity);
            _dbContext.SaveChanges();

            List<OrderItemDto> orderItemsDto = new List<OrderItemDto>();

            foreach(var orderItem in orderItems)
            {
                orderItemsDto.Add(new OrderItemDto
                {
                    Id = orderItem.Id,
                    ProductIdentifier = orderItem.ProductIdentifier,
                    Quantity = orderItem.Quantity,
                    UnitPrice = orderItem.UnitPrice,
                    TotalPrice = orderItem.Quantity * orderItem.UnitPrice
                });
            }
            
            var createdOrder = _dbContext.Orders
                                .Select(o => 
                                    new OrderDto
                                    {
                                        Id = o.Id,
                                        Email = o.Email,
                                        FullName = o.FullName,
                                        StreetName = o.StreetName,
                                        HouseNumber = o.HouseNumber,
                                        ZipCode = o.ZipCode,
                                        Country = o.Country,
                                        City = o.City,
                                        CardholderName = o.CardHolderName,
                                        CreditCard = o.MaskedCreditCard,
                                        OrderDate = o.OrderDate.ToString("dd/MM/yyyy").Replace("- ", ".").Replace(" ", ""), // • Represented as 01.01.2020
                                        TotalPrice = o.TotalPrice,
                                        OrderItems = orderItemsDto
                                    }
                                ).FirstOrDefault(o => o.Id == entity.Id);
            
            createdOrder.CreditCard = payment_card.CardNumber;
            return createdOrder;
        }
    }
}