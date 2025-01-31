using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.ProductApi.Data;
using Shop.Services.ProductAPI.Models;
using Shop.Services.ProductAPI.Models.Dto;

namespace Shop.Services.ProductAPI.Controllers
{
    [Route("api/ProductApi")]
    [ApiController]

    public class ProductApiController : ControllerBase

    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        public ProductApiController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> products = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
                return _response;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product products = _db.Products.FirstOrDefault(p => p.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(products);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto body)
        {
            try
            {
                var product = _mapper.Map<Product>(body);
                _db.Products.Update(product);
                _db.SaveChanges();

                _response.Result = _mapper.Map<Product>(product);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto body)
        {
            try
            {
                var product = _mapper.Map<Product>(body);
                _db.Products.Add(product);
                _db.SaveChanges();

                _response.Result = _mapper.Map<Product>(product);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product product = _db.Products.First(c => c.ProductId == id);
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}