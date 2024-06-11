using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
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
