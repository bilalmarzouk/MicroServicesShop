﻿using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.Interfaces;
using System.Text;
using static Shop.Web.Utility.Details;


namespace Shop.Web.Service
{
    public class BaseService:IBaseService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory,ITokenProvider tokenPRovider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenPRovider;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestdto, bool withBearer = true)
        {
            HttpClient client = _httpClientFactory.CreateClient("ShopApi");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            //token
            if(withBearer)
            {
                var token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization",$"Bearer {token}");
            }

            message.RequestUri = new Uri(requestdto.Url);

            if (requestdto.Data != null)
            {
                var test = JsonConvert.SerializeObject(requestdto.Data);
                message.Content = new StringContent(JsonConvert.SerializeObject(requestdto.Data), Encoding.UTF8, "application/json");
            }
            HttpResponseMessage? apiResponse = null;

            try { 
            switch (requestdto.ApiType)
            {
                case ApiType.POST: message.Method = HttpMethod.Post; break;
                case ApiType.PUT: message.Method = HttpMethod.Put; break;
                case ApiType.DELETE: message.Method = HttpMethod.Delete; break;
                default: message.Method = HttpMethod.Get; break;
            }
            apiResponse = await client.SendAsync(message);
                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound: return new ResponseDto() { IsSuccess = false, Message = "NotFound" };
                    case System.Net.HttpStatusCode.Unauthorized: return new ResponseDto() { IsSuccess = false, Message = "Unauthorized" };
                    case System.Net.HttpStatusCode.Forbidden: return new ResponseDto() { IsSuccess = false, Message = "Forbidden" };
                    case System.Net.HttpStatusCode.InternalServerError: return new ResponseDto() { IsSuccess = false, Message = "InternalServerError" };
                    default: var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResonseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResonseDto;
                }

            }
            catch(Exception ex)
            {
                var dto = new ResponseDto()
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false 
                };
                return dto;
            }
        }

    }
}
