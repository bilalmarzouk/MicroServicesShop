using Microsoft.AspNetCore.Identity;
using Shop.Services.AuthApi.Data;
using Shop.Services.AuthApi.Model;
using Shop.Services.AuthApi.Model.Dto;
using Shop.Services.AuthApi.Service.Interfaces;

namespace Shop.Services.AuthApi.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(ApplicationDbContext db, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
