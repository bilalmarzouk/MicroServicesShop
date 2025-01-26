using Newtonsoft.Json.Linq;
using Shop.Web.Service.Interfaces;
using Shop.Web.Utility;

namespace Shop.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(Details.TokenCookie);
        }

        public string? GetToken()
        {
            string? token = null;
            if (_contextAccessor.HttpContext != null)
            {
                bool hasToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(Details.TokenCookie, out token);
                return hasToken ? token : null;
            }
            return null;

        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(Details.TokenCookie, token);
        }
    }
}
