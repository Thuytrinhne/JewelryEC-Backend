using JewelryEC_Backend.Models.Auths.Entities;

namespace JewelryEC_Backend.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
        ApplicationUser ValidateToken (string token);
    }
}
