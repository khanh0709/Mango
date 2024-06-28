using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.Extensions.DependencyInjection;

namespace Lib
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection service)
        {
            service.AddScoped<ITokenProvider, TokenProvider>();
            service.AddScoped<IBaseService, BaseService>();
            service.AddScoped<ICouponService, CouponService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<ICartService, CartService>();
            return service;
        }
    }
}
