using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartService _cartService;	
		public CartController(ICartService cartService)
		{
			_cartService = cartService;
		}
	
		public async Task<IActionResult> CartIndex()
		{
			return View(await LoadCart());
		}
		private async Task<CartDTO> LoadCart()
		{
			var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
			ResponseDto response = await _cartService.GetCartByUserIdAsync(userId);
			if(response != null && response.IsSuccess)
			{
				CartDTO cartDto = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
				return cartDto;
			}
			return new CartDTO();
		}
	}
}
