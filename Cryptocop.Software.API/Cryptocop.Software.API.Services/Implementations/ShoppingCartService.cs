using Cryptocop.Software.API.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, ICryptoCurrencyService cryptoCurrencyService = null)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _cryptoCurrencyService = cryptoCurrencyService;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _shoppingCartRepository.GetCartItems(email);
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem)
        {
            var priceInUsd = findCryptoUsd(shoppingCartItemItem.ProductIdentifier);
            _shoppingCartRepository.AddCartItem(email, shoppingCartItemItem, priceInUsd.Result);
        }

        public void RemoveCartItem(string email, int id)
        {
            _shoppingCartRepository.RemoveCartItem(email, id);
            return;
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            _shoppingCartRepository.UpdateCartItemQuantity(email, id, quantity);
            return;
        }

        public void ClearCart(string email)
        {
            _shoppingCartRepository.ClearCart(email);
            return;
        }


        private async Task<float> findCryptoUsd(string cryptoName)
        {
            var cryptoCurr = await _cryptoCurrencyService.GetCryptoToUsd(cryptoName);
            return cryptoCurr.PriceInUsd;
        }
    }
}
