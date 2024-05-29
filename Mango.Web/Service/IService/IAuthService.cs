using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Microsoft.AspNetCore.Identity.Data;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponseDto> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponseDto> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO);

    }
}
