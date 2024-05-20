using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext db;
        private ResponseDto response;
        public CouponAPIController(AppDbContext appDb) 
        { 
            db=appDb;
            response = new ResponseDto();
        }
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = db.Coupons.ToList();
                response.Result = objList;  
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message; 
            }
            return response; 
        }

        [HttpGet]
        [Route("/{id:int}")]
        public ResponseDto Get(int id) 
        {
            try
            {
                Coupon obj = db.Coupons.First(c => c.CouponId == id);
                response.Result = obj;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
