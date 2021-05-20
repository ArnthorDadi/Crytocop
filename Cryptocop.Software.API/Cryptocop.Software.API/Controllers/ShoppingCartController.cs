using System;
using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetShoppingCartItem()
        {   
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            return Ok(_shoppingCartService.GetCartItems(email));
        }

        // • ShoppingCartController (4%)
        //    • /api/cart [POST] - Adds an item to the shopping cart, see Models section for reference
        [HttpPost]
        [Route("")]
        public IActionResult AddShoppingCartItem([FromBody] ShoppingCartItemInputModel shoppingCartItemItem)
        {
            if(!ModelState.IsValid){ return BadRequest(); }
            
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            _shoppingCartService.AddCartItem(email, shoppingCartItemItem);
            return NoContent();
        }
        //    • /api/cart/{id} [DELETE] - Deletes an item from the shopping cart
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteShoppingCartItem(int id)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            _shoppingCartService.RemoveCartItem(email, id);
            return NoContent();
        }
        //    • /api/cart/{id} [PATCH] - Updates the quantity for a shopping cart item
        [HttpPatch]
        [Route("{id}")]
        public IActionResult PatchShoppingCartItemQuantity([FromQuery] float quantity, int id)
        {
            if(quantity < 0.009){ return BadRequest(); }
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            _shoppingCartService.UpdateCartItemQuantity(email, id, quantity);
            return NoContent();
        }
        //    • /api/cart [DELETE] - Clears the cart - all items within the cart should be deleted+[HttpPatch]
        [HttpDelete]
        [Route("")]
        public IActionResult DeleteShoppingCart()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            _shoppingCartService.ClearCart(email);
            return NoContent();
        }
    }
}