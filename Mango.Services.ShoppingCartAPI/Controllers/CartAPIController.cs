using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class CartAPIController : ControllerBase
	{
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private ResponseDto _response;
        public CartAPIController(AppDbContext db, IMapper mapper, IProductService productService, ICouponService couponService)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _productService = productService;
            _couponService = couponService;
        }
        //    [HttpPost("CartUpsert")]
        //    public async Task<ResponseDto> CartUpsert(CartDTO cartDTO)
        //    {
        //        try
        //        {
        //            CartHeader? cartHeader = await _db.CartHeader.FirstOrDefaultAsync(c => c.UserId == cartDTO.CartHeader.UserId);    
        //            if(cartHeader == null)
        //            {
        //	CartHeader cart =  _mapper.Map<CartHeader>(cartDTO.CartHeader);
        //                _db.CartHeader.Add(cart);
        //                await _db.SaveChangesAsync();

        //                cartDTO.CartDetails.First().CartHeaderId = cart.CartHeaderId;
        //                _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
        //                await _db.SaveChangesAsync();
        //}
        //else
        //            {
        //                var cartDetail = await _db.CartDetails.FirstOrDefaultAsync(c => c.CartHeaderId == cartHeader.CartHeaderId && cartDTO.CartDetails.First().ProductId == c.ProductId);
        //                if(cartDetail == null) {
        //                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
        //                    _db.SaveChanges();
        //                }
        //                else 
        //                {
        //                    cartDetail.Count += cartDTO.CartDetails.First().Count;
        //                    _db.CartDetails.Update(cartDetail);
        //                    //nếu muốn update dto thì phải thêm asnotracking khi query
        //                    _db.SaveChanges();
        //                }
        //            }
        //            _response.IsSuccess = true;
        //            _response.Result = null;
        //        }
        //        catch (Exception ex)
        //        {
        //            _response.IsSuccess = false;
        //            _response.Result = null;
        //        }
        //        return _response;
        //    }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDTO cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(_db.CartHeader.First(c => c.UserId == userId)),

                };
                var detail = _mapper.Map<CartDetailsDTO>(_db.CartDetails.First(c => c.CartHeaderId == cart.CartHeader.CartHeaderId));
                var details = _db.CartDetails.Where(c => c.CartHeaderId == cart.CartHeader.CartHeaderId);
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDTO>>(details);
                IEnumerable<ProductDTO> productDtos = await _productService.GetProducts();
                foreach(var item in cart.CartDetails) 
                {
                    item.Product = productDtos.FirstOrDefault(p => p.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);    
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                    //giam 500k(discountAmount) cho don tu 1tr(minAmount)
                }
                _response.Result = cart;
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDTO cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeader.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeader.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(c => c.CartDetailsId == cartDetailsId);
                int countOfCart = _db.CartDetails.Where(c => c.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if(countOfCart == 1)
                {
                    CartHeader cartHeader = await _db.CartHeader.FirstOrDefaultAsync(c => c.CartHeaderId == cartDetails.CartHeaderId);
                }
                await _db.SaveChangesAsync();
                _response.Result = null;
            }
            catch (Exception e)
            {
                _response.Message =e.Message.ToString();    
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromBody = await _db.CartHeader.FirstAsync(u => u.UserId == cartDTO.CartHeader.UserId);
                cartFromBody.CouponCode = cartDTO.CartHeader.CouponCode;
                _db.CartHeader.Update(cartFromBody);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromDb = await _db.CartHeader.FirstAsync(u => u.UserId == cartDTO.CartHeader.UserId);
                cartFromDb.CouponCode = "";
                _db.CartHeader.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.ToString();
			}
			return _response;
		}
    }
}
