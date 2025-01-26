using Shop.Web.Models;

namespace Shop.Web.Service.Interfaces
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestdto,bool withBearer = true);
    }
}
