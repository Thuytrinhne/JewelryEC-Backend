using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Users.Dto;

namespace JewelryEC_Backend.Service.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> ListUsers( Guid roleId);
        ApplicationUser GetUserById(Guid idUser);
        Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user);
        ApplicationUser EditProfile(Guid UserId, UpdateUserDto user);
        Task<bool> AssignRole(Guid userId, Guid roleId);
    }
}
