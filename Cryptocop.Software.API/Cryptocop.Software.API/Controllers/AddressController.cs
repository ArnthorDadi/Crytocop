using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressSerice;

        public AddressController(IAddressService addressSerice)
        {
            _addressSerice = addressSerice;
        }

        // • AddressController (2%)
        //     • /api/addresses [GET] - Gets all addresses associated with authenticated user
        [HttpGet]
        [Route("")]
        public IActionResult GetAllAddresses()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            return Ok(_addressSerice.GetAllAddresses(email));
        }
        //     • /api/addresses [POST] - Adds a new address associated with authenticated user, see
        //     Models section for reference
        [HttpPost]
        [Route("")]
        public IActionResult AddAddress([FromBody] AddressInputModel address)
        {
            if(!ModelState.IsValid){ return BadRequest(); }

            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            _addressSerice.AddAddress(email, address);
            return Ok();
        }
        // • /api/addresses/{id} [DELETE] - Deletes an address by id
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteAddress(int id)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            _addressSerice.DeleteAddress(email, id);
            return Ok();
        }
    }
}