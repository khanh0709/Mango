namespace Mango.Web.Util
{
    public class SD
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string RoleAdmin = "ADMIN";
        public static string RoleCustomer = "CUSTOMER";
        public enum ApiType
        {
            Get,
            Post,
            Put,
            Delete
        }
    }
}
