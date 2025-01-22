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
        private IJWTTokenGenrator _jwtTokenGenrator;
        public AuthService(ApplicationDbContext db, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager, IJWTTokenGenrator jwtTokenGenrator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenrator = jwtTokenGenrator;
        }

        public async Task<bool> AssignRole(string email, string rolename)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if ((user != null))
            {
                if(!_roleManager.RoleExistsAsync(rolename).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(rolename)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, rolename);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isvalid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (isvalid == false || user == null)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }
            var token =_jwtTokenGenrator.GenrateToken(user);

            UserDTO userDto = new UserDTO()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumer = user.PhoneNumber
            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUsers user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name,
                PhoneNumber = registrationRequestDTO.PhoneNumber,
            };
            try
            {
                var result =await  _userManager.CreateAsync(user,registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    var userResult = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDTO.Email);
                    UserDTO userDTO = new()
                    {
                        Email = userResult.Email,
                        Id = userResult.Id,
                        PhoneNumer = userResult.PhoneNumber,
                        Name = userResult.Name,
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description ;
                }
            }
            catch (Exception ex)
            {
            }
            return "error encouterd";
        }
    }
}
