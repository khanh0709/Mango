﻿using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();   
                config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();   
            });
            return mappingConfig;
        }
    }
}
