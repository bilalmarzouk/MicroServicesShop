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
        public ResponseDto Put(ProductDto body)
        {
            try
            {
                var product = _mapper.Map<Product>(body);
                if (body.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePAthDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePAthDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    string fileName = product.ProductId + Path.GetExtension(body.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var filestream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        body.Image.CopyTo(filestream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;

                }

            
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
        public ResponseDto Post(ProductDto body)
        {
            try
            {
                var product = _mapper.Map<Product>(body);
                _db.Products.Add(product);
                _db.SaveChanges();

                if (body.Image != null)
                {
                 
                    string fileName = product.ProductId + Path.GetExtension(body.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var filestream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        body.Image.CopyTo(filestream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                  
                }
             
                else
                {
                    product.ImageUrl = "https://placehold.co/600*400";
                }
                _db.Products.Update(product);
                _db.SaveChanges();
                _response.Result = _mapper.Map<Product>(product);
            
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
                if(!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    var oldFilePAthDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePAthDirectory);
                    if(file.Exists)
                    {
                        file.Delete();
                    }
                }
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