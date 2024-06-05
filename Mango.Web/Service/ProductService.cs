using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Util;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService) 
        {
            _baseService = baseService;
        } 
        public Task<ResponseDto> CreateProductAsync(ProductDTO productDTO)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Post,
                Url = SD.ProductAPIBase + "/api/ProductAPI/",
                Data = productDTO
            }, withBearer: true);
        }

        public Task<ResponseDto> DeleteProductAsync(int id)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Delete,
                Url = SD.ProductAPIBase + "/api/ProductAPI/" + id,
            }, withBearer: true);
        }

        public Task<ResponseDto> GetAllProductAsync()
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Get,
                Url = SD.ProductAPIBase + "/api/ProductAPI/",
            }, withBearer: true);
        }

        public Task<ResponseDto> GetProductByIdAsync(int id)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Get,
                Url = SD.ProductAPIBase + "/api/ProductAPI/"+id,
            }, withBearer: true);
        }

        public Task<ResponseDto> UpdateProductAsync(ProductDTO productDTO)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.Put,
                Url = SD.ProductAPIBase + "/api/ProductAPI/",
                Data = productDTO
            }, withBearer: true);
        }
    }
}
