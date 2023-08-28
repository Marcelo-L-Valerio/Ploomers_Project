using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ploomers_Project_API.Business;
using Ploomers_Project_API.Mappers.DTOs.InputModels;

namespace Ploomers_Project_API.Controllers
{
    [ApiVersion("1")]
    [Route("api/auth/v{version:apiVersion}")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        // POST api/auth/v1/signin - gives the JWT user data for the session
        [HttpPost]
        [Route("signin")]
        public IActionResult Signin(LoginInputModel user)
        {
            if (user == null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidateCredentials(user);

            if (token == null) return Unauthorized();
            return Ok(token);
        }

        // POST api/auth/v1/refresh - gives the JWT user refresh token data
        // for an expired session
        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenInputModel tokenDto)
        {
            if (tokenDto == null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidateCredentials(tokenDto);

            if (token == null) return BadRequest("Invalid client request");
            return Ok(token);
        }

        // GET api/auth/v1/revoke - logs the user out and invalidates his tokens
        [HttpGet]
        [Authorize("Bearer")]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var email = User.Identity.Name;
            var result = _loginBusiness.RevokeToken(email);

            if (result == false) return BadRequest("Invalid client request");
            return NoContent();
        }
    }
}
