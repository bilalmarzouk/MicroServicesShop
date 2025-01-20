using Microsoft.AspNetCore.Identity;

namespace Shop.Services.AuthApi.Model
{
    public class ApplicationUsers : IdentityUser
    {
        public string Name { get; set; }
    }
}
