using Microsoft.AspNetCore.Mvc;
using Shop.Web.Models;
using Shop.Web.Service.Interfaces;

namespace Shop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
       [HttpGet]
       public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new ();
            return View(loginRequestDto);
        }
        [HttpGet]
        public IActionResult Register()
        {
       
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
      
            return View();
        }
    }
}
