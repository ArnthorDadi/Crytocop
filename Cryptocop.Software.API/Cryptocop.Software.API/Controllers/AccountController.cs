using System;
using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        // • AccountController (3%)
        // TODO: Setup routes

        // • /api/account/register [POST] - Registers a user within the application, see Models
        //                                  section for reference
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterInputModel register)
        {
            if(!ModelState.IsValid){ return BadRequest(); }

            var user = _accountService.CreateUser(register);
            if (user == null) { return Unauthorized(); }

            return Ok(user);
        }

        // • /api/account/signin [POST] - Signs the user in by checking the credentials provided
        //                                  and issuing a JWT token in return, see Models section for reference
        [AllowAnonymous]
        [HttpPost]
        [Route("singin")]
        public IActionResult SignIn([FromBody] LoginInputModel login)
        {
            if(!ModelState.IsValid){ return BadRequest(); }

            var user = _accountService.AuthenticateUser(login);
            if (user == null) { return Unauthorized(); }

            return Ok(_tokenService.GenerateJwtToken(user));
        }
        // • /api/account/signout [GET] - Logs the user out by voiding the provided JWT token
        //                                using the id found within the claim
        [HttpGet]
        [Route("signout")]
        public IActionResult SignOut()
        {
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "tokenId").Value, out var tokenId);
            _accountService.Logout(tokenId);
            return Ok(tokenId);
        }

        /* To see the claim, remove before submitting */
        [HttpGet]
        [Route("userinfo")]
        public IActionResult GetUserInfo()
        {
            var claims = User.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });
            return Ok(claims);
        }
    }
}