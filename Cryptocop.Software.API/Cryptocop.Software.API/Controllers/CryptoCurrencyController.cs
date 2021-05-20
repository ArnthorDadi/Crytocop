using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Services.Helpers;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cryptocurrencies")]
    public class CryptoCurrencyController : ControllerBase
    {
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public CryptoCurrencyController(ICryptoCurrencyService cryptoCurrencyService)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
        }

        // • CryptocurrencyController (1%)
        //    • /api/cryptocurrencies [GET] - Gets all available cryptocurrencies - the only available
        //      cryptocurrencies in this platform are BitCoin (BTC), Ethereum (ETH), Tether (USDT) and
        //      Monero (XMR)
        [HttpGet]
        [Route("")]
        public IActionResult GetCryptoCurrencies()
        {
            return Ok(_cryptoCurrencyService.GetAvailableCryptocurrencies());
        }
    }
}
