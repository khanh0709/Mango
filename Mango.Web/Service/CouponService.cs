using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Util;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;
        public CouponService(IBaseService baseServiceApp)
        {
            baseService = baseServiceApp;
        }
        public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Post,
                Url = SD.CouponAPIBase + "/api/couponAPI/",
                Data = couponDto    
            });
        }

        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Delete,
                Url = SD.CouponAPIBase + "/api/couponAPI/" + id
            });
        }

        public async Task<ResponseDto> GetAllCouponAsync()
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Get,
                Url = SD.CouponAPIBase + "/api/couponAPI"
            });
        }

        public async Task<ResponseDto> GetCouponAsync(string couponCode)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Get,
                Url = SD.CouponAPIBase + "/api/couponAPI/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Get,
                Url = SD.CouponAPIBase + "/api/couponAPI/" + id
            });
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Put,
                Url = SD.CouponAPIBase + "/api/couponAPI/",
                Data = couponDto
            });
        }
    }
}
