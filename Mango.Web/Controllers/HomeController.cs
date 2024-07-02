using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService; 
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO> products = new List<ProductDTO>();
            ResponseDto responseDTO = await _productService.GetAllProductAsync();
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responseDTO.Result));
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            return View(products);
        }

        
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDTO product = new ProductDTO();
            ResponseDto responseDTO = await _productService.GetProductByIdAsync(productId);
            if(responseDTO != null && responseDTO.IsSuccess) 
            {
                product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responseDTO.Result));
			}
            else
            {
                TempData["error"] = "Error";
            }
            return View(product);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDTO productDTO)
        {
            CartDTO cartDTO = new CartDTO()
            {
                CartHeader = new CartHeaderDTO
                {
                    UserId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value,
                    //UserId = User.Claims.FirstOrDefault(u => u.Type == JwtClaimTypes.Subject).Value
                }
            };
            CartDetailsDTO cartDetailsDTO = new CartDetailsDTO()
            {
                Count = productDTO.Count,
                ProductId = productDTO.ProductId
            };
            List<CartDetailsDTO> cartDetailsDTOs = new() { cartDetailsDTO };
            cartDTO.CartDetails = cartDetailsDTOs;
            ResponseDto? responseDTO = await _cartService.UpsertCartAsync(cartDTO);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Item hass been added";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Error";
            }
            return View(productDTO);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
