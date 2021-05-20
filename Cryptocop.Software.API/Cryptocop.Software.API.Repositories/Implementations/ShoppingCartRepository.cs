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
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public ShoppingCartRepository(CryptocopDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            var user =_userRepository.findUserByEmail(email);
            if(user == null){ return null; }
            
            var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(s => s.UserId == user.Id);
            if(shoppingCart == null){ return new List<ShoppingCartItemDto>{}; }

            return _dbContext.ShoppingCartItems
                    .Where(sc => sc.ShoppingCartId == shoppingCart.Id)
                    .Select(sc => 
                        new ShoppingCartItemDto
                        {
                            Id = sc.Id,
                            ShoppingCartId = sc.ShoppingCartId,
                            ProductIdentifier = sc.ProductIdentifier,
                            Quantity = sc.Quantity,
                            UnitPrice =  sc.UnitPrice,
                            TotalPrice = (float) sc.Quantity * sc.UnitPrice
                        }
                    ).ToList();
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {
            var user =_userRepository.findUserByEmail(email);
            if(user == null){ return; }
            
            var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(s => s.UserId == user.Id);
            // Check if user does have a cart
            if(shoppingCart == null)
            { 
                // User does not have cart so lets create a cart for the user :)
                var createdShoppingCart = new ShoppingCart
                {
                    UserId = user.Id
                };
                _dbContext.Add(createdShoppingCart);
                _dbContext.SaveChanges();
                shoppingCart = createdShoppingCart;
            }

            // User exists and has a Cart
            var entity = new ShoppingCartItem
            {
                ShoppingCartId = shoppingCart.Id,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = (float) shoppingCartItemItem.Quantity,
                UnitPrice = priceInUsd,
            };
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return;
        }

        public void RemoveCartItem(string email, int id)
        {
            var cart = findCartByEmail(email);
            if(cart == null){ return; }
            var cartItem = findCartItemByEmailAndId(email, id);
            // The cartItemId that are in the users cart do not match the given cartItemId
            if(cartItem == null){ return; }
            if(cartItem.ShoppingCartId != cart.Id){ return; }

            _dbContext.Remove(cartItem);
            _dbContext.SaveChanges();
            return;
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            var cart = findCartByEmail(email);
            if(cart == null){ return; }
            var cartItem = findCartItemByEmailAndId(email, id);
            // The cartItemId that are in the users cart do not match the given cartItemId
            if(cartItem == null){ return; }
            if(cartItem.ShoppingCartId != cart.Id){ return; }
            cartItem.Quantity = quantity;
            _dbContext.SaveChanges();
            return;
        }

        public void ClearCart(string email)
        {
            var cart = findCartByEmail(email);
            if(cart == null){ return; }

            var allCartItems = _dbContext.ShoppingCartItems
                                    .Where(sc => sc.ShoppingCartId == cart.Id).ToList();
            _dbContext.RemoveRange(allCartItems);
            _dbContext.SaveChanges();
        }

        public void DeleteCart(string email)
        {
            ClearCart(email);

            var cart = findCartByEmail(email);
            if(cart == null){ return; }
            
            _dbContext.Remove(cart);
            _dbContext.SaveChanges();
        }

        private ShoppingCart findCartByEmail(string email)
        {
            // ID MUST BE A SHOPPING CART ITEM ID
            var user =_userRepository.findUserByEmail(email);
            if(user == null){ return null; }
            
            var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(s => s.UserId == user.Id);
            return shoppingCart;
        }

        private ShoppingCartItem findCartItemByEmailAndId(string email, int id)
        {
            var shoppingCart = findCartByEmail(email);
            if(shoppingCart == null){ return null; }

            // User exists and has a shopping cart
            var cartItem = _dbContext.ShoppingCartItems.FirstOrDefault(sc => 
                                                            sc.ShoppingCartId == shoppingCart.Id 
                                                            && sc.Id == id);
            // The cartItemId that are in the users cart do not match the given cartItemId
            if(cartItem == null){ return null; }
            return cartItem;
        }
    }
}