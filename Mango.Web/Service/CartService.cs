using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Mango.Web.Util;
using System.Runtime.CompilerServices;

namespace Mango.Web.Service
{
	public class CartService : ICartService
	{
		private readonly IBaseService _baseService;	
		public CartService(IBaseService baseService) 
		{
			_baseService = baseService;
		}
		public async Task<ResponseDto> ApplyCouponAsync(CartDTO cartDTO)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.Post,
				Data = cartDTO,
				Url = SD.ShoppingCartAPIBase + "/api/CartAPI/ApplyCoupon"
            });
		}

		public async Task<ResponseDto> GetCartByUserIdAsync(string userId)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.Get,
				Data = userId,
				Url = SD.ShoppingCartAPIBase + "/api/CartAPI/GetCart/" + userId
			});
		}

		public async Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.Post,
				Data = cartDetailsId,
				Url = SD.ShoppingCartAPIBase + "/api/CartAPI/RemoveCart"
            });
		}

		public async Task<ResponseDto> UpsertCartAsync(CartDTO cartDTO)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.Post,
				Data = cartDTO,
				Url = SD.ShoppingCartAPIBase + "/api/CartAPI/CartUpsert"
            });
		}
	}
}
