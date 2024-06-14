using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
	public class CartDetailsDTO
	{
		public int CartDetailsId { get; set; }
		public int CartHeaderId { get; set; }
		public CartDetailsDTO? CartHeader { get; set; }
		public int ProductId { get; set; }
		public ProductDTO? Product { get; set; }
		public int Count { get; set; }
	}
}
