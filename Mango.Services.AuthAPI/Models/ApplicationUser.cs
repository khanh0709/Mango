using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }    
    }
    //tao ra applicationUser extend identityUser va co them thuoc tinh. Nhung dieu nay ko lam tao them bang. Ma no se them 1 cot
    //name cho bang IdentityUser strong DB
}
