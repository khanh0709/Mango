using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
	public class CouponService : ICouponService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		public CouponService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task<CouponDto> GetCoupon(string couponCode)
		{
			var client = _httpClientFactory.CreateClient("Coupon");
			var response = await client.GetAsync($"/api/couponAPI"); //đã config cho client product nên có sẵn baseurl
			var apiContent = await response.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
			if (res.IsSuccess)
			{
				return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(res.Result));
			}
			return new CouponDto();
		}
	}
}
