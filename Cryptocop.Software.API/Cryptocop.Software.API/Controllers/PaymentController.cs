using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        //• PaymentController (2%)
        //    • /api/payments [GET] - Gets all payment cards associated with the authenticated user
        [HttpGet]
        [Route("")]
        public IActionResult GetAllPaymentCard()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            return Ok(_paymentService.GetStoredPaymentCards(email));
        }
        //    • /api/payments [POST] - Adds a new payment card associated with the authenticated
        //    user, see Models section for reference
        [HttpPost]
        [Route("")]
        public IActionResult AddPaymentCard([FromBody] PaymentCardInputModel payment_card)
        {
            if(!ModelState.IsValid){ return BadRequest(); }
            
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            _paymentService.AddPaymentCard(email, payment_card);
            return NoContent();
        }
    }
}