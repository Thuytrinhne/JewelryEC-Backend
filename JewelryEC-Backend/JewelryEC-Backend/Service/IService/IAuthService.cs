using JewelryEC_Backend.Models.Auths.Dto;

namespace JewelryEC_Backend.Service.IService
{
    public interface IAuthService
    {

        Task<bool> Register(RegistrationDto registrationDto);
        Task<LoginResponseDto> Login(LoginDto loginDto);
        Task<bool> AssignRole(Guid userId, Guid roleId);
        Task<bool> SendingOTP(string email);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(string resetToken, string newPass);



    }
}
