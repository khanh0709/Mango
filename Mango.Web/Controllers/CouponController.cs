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
            List<CouponDto> list = null;
            ResponseDto response = await couponService.GetAllCouponAsync();
            if(response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
			{
				TempData["error"] = "Error";
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
                    TempData["error"] = "Error";
                }
            }

			return View();
		}

		public async Task<IActionResult> CouponDelete(int couponId)
		{
			ResponseDto response = await couponService.GetCouponByIdAsync(couponId);
			if (response != null && response.IsSuccess)
			{
                CouponDto couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(couponDto);
			}
            else
            {
                TempData["error"] = "Error";
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
                TempData["error"] = "Error";
            }
            return RedirectToAction("CouponIndex");
		}
	}
}
