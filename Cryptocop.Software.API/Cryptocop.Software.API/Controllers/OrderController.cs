using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // • OrderController (2%)
        //      • /api/orders [GET] - Gets all orders associated with the authenticated user
        [HttpGet]
        [Route("")]
        public IActionResult GetAllOrders()
        {   
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            return Ok(_orderService.GetOrders(email));
        }
        //      • /api/orders [POST] - Adds a new order associated with the authenticated user, see
        //      Models section for reference
        [HttpPost]
        [Route("")]
        public IActionResult AddOrder([FromBody] OrderInputModel order)
        {
            if(!ModelState.IsValid){ return BadRequest(); }

            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            _orderService.CreateNewOrder(email, order);
            return NoContent();
        }
    }
}