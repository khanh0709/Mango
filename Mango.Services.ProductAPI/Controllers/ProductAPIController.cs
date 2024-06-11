using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext db;
        private ResponseDto response;
        private IMapper mapper;
        public ProductAPIController(AppDbContext appDb, IMapper appMapper) 
        { 
            db = appDb;
            mapper = appMapper;
            response = new ResponseDto();
        }
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> objList = db.Products.ToList();
                response.Result = mapper.Map<IEnumerable<ProductDTO>>(objList);  
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message; 
            }
            return response; 
        }

        [HttpGet]
        [Route("{id}")]
        public ResponseDto Get(int id) 
        {
            try
            {
                Product obj = db.Products.First(c => c.ProductId == id);
                response.Result = mapper.Map<ProductDTO>(obj);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        //[HttpGet]
        //[Route("GetByCode/{code}")]
        //public ResponseDto GetByCode(string code)
        //{
        //    try
        //    {
        //        Coupon obj = db.Coupons.First(c => c.CouponCode == code);
        //        response.Result = mapper.Map<CouponDto>(obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody]ProductDTO productDTO)
        {
            try
            {
                Product product = mapper.Map<Product>(productDTO);
                db.Products.Add(product);
                db.SaveChanges();

                response.Result = productDTO;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDTO producDto)
        {
            try
            {
                Product product = mapper.Map<Product>(producDto);
                db.Update(product);
                db.SaveChanges();

                response.Result = producDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product product = db.Products.First(c => c.ProductId == id);
                db.Remove(product);
                db.SaveChanges();   
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
