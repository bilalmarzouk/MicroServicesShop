using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.CouponAPI.Data;
using Shop.Services.CouponAPI.Models;
using Shop.Services.CouponAPI.Models.Dto;

namespace Shop.Services.CouponAPI.Controllers
{
    [Route("api/CouponApi")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        public CouponAPIController(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
           
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> result = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(result);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route ("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
               Coupon result = _db.Coupons.First(c=>c.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(result);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto Get(string code)
        {
            try
            {
                Coupon result = _db.Coupons.First(c => c.CopounCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(result);
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
        public ResponseDto Post([FromBody] CouponDto coupondto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(coupondto);
                _db.Coupons.Add(coupon);
                _db.SaveChanges();

               
                _response.Result = _mapper.Map<CouponDto>(coupon);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto coupondto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(coupondto);
                _db.Coupons.Update(coupon);
                _db.SaveChanges();


                _response.Result = _mapper.Map<CouponDto>(coupon);
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
                Coupon result = _db.Coupons.First(c => c.CouponId == id);
              
                _db.Coupons.Remove(result);
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
