using JewelryEC_Backend.Models.Auths.Dto;

namespace JewelryEC_Backend.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationDto registrationDto);
        Task<LoginResponseDto> Login(LoginDto loginDto);
        Task<bool> AssignRole(string id, string roleName);

    }
}
