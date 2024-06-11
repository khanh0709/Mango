using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService couponService;
        public CouponController(ICouponService couponServiceApp)
        {
            couponService = couponServiceApp;   
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list = new List<CouponDto>();
            ResponseDto response = await couponService.GetAllCouponAsync();
            if(response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
			{
				TempData["error"] = response.Message;
			}
            return View(list);
        }

		public IActionResult CouponCreate()
		{
            return View();
		}

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
			if(ModelState.IsValid)
            {
				ResponseDto response = await couponService.CreateCouponAsync(couponDto);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Success";
					return RedirectToAction("CouponIndex");
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }

			return View();
		}

		public async Task<IActionResult> CouponDelete(int couponId)
		{
			ResponseDto response = await couponService.GetCouponByIdAsync(couponId);
			if (response != null && response.IsSuccess)
			{
                CouponDto? couponDto = JsonConvert.DeserializeObject<CouponDto>(response.Result.ToString());
                return View(couponDto);
			}
            else
            {
                TempData["error"] = response.Message;
            }

            return RedirectToAction("CouponIndex");
        }

		public async Task<IActionResult> CouponDeleteHandler(int couponId)
		{
			ResponseDto response = await couponService.DeleteCouponAsync(couponId);
			if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Success";
                return Redirect("CouponIndex");
			}
            else
            {
                TempData["error"] = response.Message;
            }
            return RedirectToAction("CouponIndex");
		}
	}
}
