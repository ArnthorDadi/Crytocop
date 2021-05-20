using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/exchanges")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        // • ExchangeController (1%)
        //    • /api/exchanges [GET] - Gets all exchanges in a paginated envelope. This routes
        //    accepts a single query parameter called pageNumber which is used to paginate the
        //    results 
        [HttpGet]
        [Route("")]
        public IActionResult GetCryptoCurrencies([FromQuery] int pageNumber = 1)
        {
            return Ok(_exchangeService.GetExchanges(pageNumber).Result);
        }

    }
}