using Mango.Web.Models;
using Mango.Web.Models.Dto;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
		Task<ResponseDto> GetCartByUserIdAsync(string userId);
		Task<ResponseDto> UpsertCartAsync(CartDTO cartDTO);
		Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId);
		Task<ResponseDto> ApplyCouponAsync(CartDTO cartDTO);
	}
}
