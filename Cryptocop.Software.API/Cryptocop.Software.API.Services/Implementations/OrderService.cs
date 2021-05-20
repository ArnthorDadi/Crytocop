using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IQueueService _mbClient;

        public OrderService(IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository, IQueueService mbClient)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _mbClient = mbClient;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            return _orderRepository.GetOrders(email);
        }

        public void CreateNewOrder(string email, OrderInputModel order)
        {
            var createdOrder = _orderRepository.CreateNewOrder(email, order);
            if(createdOrder == null){ return; }
            _shoppingCartRepository.DeleteCart(email);
            _mbClient.PublishMessage("create-order", createdOrder);
            return;
        }
    }
}