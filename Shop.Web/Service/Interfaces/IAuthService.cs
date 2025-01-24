using Shop.Web.Models;

namespace Shop.Web.Service.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO registerationRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDTO registerationRequestDto);
    }
}
