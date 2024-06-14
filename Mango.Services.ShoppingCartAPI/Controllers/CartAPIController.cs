using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CartAPIController : ControllerBase
	{
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;
        public CartAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }
        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                CartHeader? cartHeader = await _db.CartHeader.FirstOrDefaultAsync(c => c.CartHeaderId == cartDTO.CartHeader.CartHeaderId);    
                if(cartHeader == null)
                {
					CartHeader cart =  _mapper.Map<CartHeader>(cartDTO.CartHeader);
                    _db.CartHeader.Add(cart);
                    await _db.SaveChangesAsync();

                    cartDTO.CartDetails.First().CartHeaderId = cart.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                    await _db.SaveChangesAsync();
				}
				else
                {
                    var cartDetail = await _db.CartDetails.FirstOrDefaultAsync(c => c.CartHeaderId == cartHeader.CartHeaderId && cartDTO.CartDetails.First().ProductId == c.ProductId);
                    if(cartDetail == null) {
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        _db.SaveChanges();
                    }
                    else 
                    {
                        cartDetail.Count += cartDTO.CartDetails.First().Count;
                        _db.CartDetails.Update(cartDetail);
                        _db.SaveChanges();
                    }
                }
                _response.IsSuccess = true;
                _response.Result = null;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = null;
            }
            return _response;
        }
    }
}
