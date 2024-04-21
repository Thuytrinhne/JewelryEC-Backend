using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Service.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> ListUsers(string roleId = "");
        ApplicationUser GetUserById(Guid idUser);
        Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user);
        ApplicationUser EditProfile(ApplicationUser user);
    }
}
