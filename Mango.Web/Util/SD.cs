﻿namespace Mango.Web.Util
{
    public class SD //static detail
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ProductAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string RoleAdmin = "ADMIN";
        public static string RoleCustomer = "CUSTOMER";
        public static string TokenCookie = "JWTToken";
        public enum ApiType
        {
            Get,
            Post,
            Put,
            Delete
        }
    }
}
