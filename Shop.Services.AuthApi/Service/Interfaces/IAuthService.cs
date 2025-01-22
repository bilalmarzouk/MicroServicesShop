using Shop.Services.AuthApi.Model.Dto;

namespace Shop.Services.AuthApi.Service.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        Task<bool> AssignRole(string email, string rolename);
    }
}
