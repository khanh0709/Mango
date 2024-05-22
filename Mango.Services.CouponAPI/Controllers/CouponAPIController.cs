﻿using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext db;
        private ResponseDto response;
        private IMapper mapper;
        public CouponAPIController(AppDbContext appDb, IMapper appMapper) 
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
                IEnumerable<Coupon> objList = db.Coupons.ToList();
                response.Result = mapper.Map<IEnumerable<CouponDto>>(objList);  
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
                Coupon obj = db.Coupons.First(c => c.CouponId == id);
                response.Result = mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon obj = db.Coupons.First(c => c.CouponCode == code);
                response.Result = mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody]CouponDto couponDto)
        {
            try
            {
                Coupon counpon = mapper.Map<Coupon>(couponDto);
                db.Coupons.Add(counpon);
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
