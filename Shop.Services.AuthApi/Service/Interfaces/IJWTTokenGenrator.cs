using Shop.Services.AuthApi.Model;

namespace Shop.Services.AuthApi.Service.Interfaces
{
    public interface IJWTTokenGenrator
    {
        string GenrateToken(ApplicationUsers applicationuser,IEnumerable<string>roles);
    }
}
