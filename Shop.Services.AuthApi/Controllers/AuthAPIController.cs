using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.AuthApi.Model.Dto;
using Shop.Services.AuthApi.Models.Dto;
using Shop.Services.AuthApi.Service.Interfaces;

namespace Shop.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        public ResponseDto _response;
        public AuthAPIController(IAuthService authService)
        {
           _authService = authService;
            _response = new();
        }

        [HttpPost("regiser")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            var responseMassage = await _authService.Register(model);
            if(!string.IsNullOrEmpty(responseMassage))
            {
                _response.IsSuccess = false;
                _response.Message = responseMassage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "USername or Password is Wrong";
                return BadRequest(_response);

            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> Login([FromBody] RegistrationRequestDTO model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encoutred";
                return BadRequest(_response);

            }
            return Ok(_response);
        }
    }
}
