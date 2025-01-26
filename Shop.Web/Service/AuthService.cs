using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;

namespace Shop.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDTO registerationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = registerationRequestDto,
                Url = Details.AuthAPIBase + $"/api/auth/AssignRole"

            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = loginRequestDto,
                Url = Details.AuthAPIBase + $"/api/auth/login"

            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO registerationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.Details.ApiType.POST,
                Data = registerationRequestDto,
                Url = Details.AuthAPIBase + $"/api/auth/register"

            }, withBearer: false);
        }
    }
}
