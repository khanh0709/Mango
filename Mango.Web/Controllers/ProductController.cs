using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;   
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO> products = new List<ProductDTO>();
            ResponseDto responseDTO = await _productService.GetAllProductAsync();    
            if(responseDTO != null && responseDTO.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responseDTO.Result));
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            return View(products);
        }
        public IActionResult ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDTO productDTO)
        {
            ResponseDto responseDTO = await _productService.CreateProductAsync(productDTO);
            if(responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Create product successfully!";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            return View();
        }
        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto responseDTO = await _productService.GetProductByIdAsync(productId);
            if(responseDTO != null && responseDTO.IsSuccess)
            {
                ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responseDTO.Result));
                return View(productDTO);
            }
            return RedirectToAction(nameof(ProductIndex));
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDTO productDTO)
        {
            ResponseDto responseDTO = await _productService.UpdateProductAsync(productDTO); 
            if(responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Edit product successfully!";
                return RedirectToAction("ProductIndex");
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto responseDTO = await _productService.GetProductByIdAsync(productId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responseDTO.Result));
                return View(productDTO);
            }
            return RedirectToAction(nameof(ProductIndex));
        }
        [HttpPost]
        public async Task<IActionResult> ProductDeleteHandler(int productId)
        {
            ResponseDto responseDTO = await _productService.DeleteProductAsync(productId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Delete product successfully!";
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            return RedirectToAction(nameof(ProductIndex));
        }
    }
}
