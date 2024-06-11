using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();    
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            ResponseDto responseDTO = await _authService.LoginAsync(loginRequestDTO);
            if(responseDTO != null && responseDTO.IsSuccess)
            {
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(responseDTO.Result));
                await SigninUser(loginResponseDTO);
                _tokenProvider.SetToken(loginResponseDTO.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDTO.Message;
                return View(loginRequestDTO);
            }
        }

        [HttpGet]
        public IActionResult Register() 
        {
            RegistrationRequestDTO registrationRequestDTO = new RegistrationRequestDTO();
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem{Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };
            ViewBag.RoleList = roleList;    
            return View(registrationRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO model)
        {
            ResponseDto responseDTO = await _authService.RegisterAsync(model);
            ResponseDto assignRole;
            if(responseDTO != null && responseDTO.IsSuccess)
            {
                if(String.IsNullOrEmpty(model.Role))
                {
                    model.Role = SD.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(model);
                    
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registratin Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem{Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };
            ViewBag.RoleList = roleList;
            return View(model);
        }

        private async Task SigninUser(LoginResponseDTO model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            //identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
            //    jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            //identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, 
            //    jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, 
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            //tai sao ko dung claimtypes o day
            //identity.AddClaim(new Claim(ClaimTypes.Name,
            //   jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            //dung de phan quyen trong controller cua Web, ko phai cua api
            identity.AddClaim(new Claim(ClaimTypes.Role,
               jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //de User.Identity.IsAuthenticated tra ve true
        }
    }
}
