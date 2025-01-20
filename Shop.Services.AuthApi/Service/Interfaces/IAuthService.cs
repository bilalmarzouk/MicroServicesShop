using Shop.Services.AuthApi.Model.Dto;

namespace Shop.Services.AuthApi.Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
