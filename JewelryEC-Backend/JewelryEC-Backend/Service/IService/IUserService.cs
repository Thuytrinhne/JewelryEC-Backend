using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Users.Dto;

namespace JewelryEC_Backend.Service.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task<PaginationResult<ApplicationUser>> GetUsersPagination(PaginationRequest request);

        ApplicationUser GetUserById(Guid idUser);
        Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user);
        Task<PaginationResult<ApplicationUser>> GetUsersByRoleIdPagination(Guid roleId, PaginationRequest request);

        ApplicationUser EditProfile(Guid UserId, UpdateUserDto user);
        Task<bool> AssignRole(Guid userId, Guid roleId);

        Task<bool> UpdatePassword ( ApplicationUser user, string currentPass, string newPass);
      
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleId(Guid roleId);

        Task<PaginationResult<ApplicationUser>> SearchRecordsAsyncPagination(PaginationRequest request, Guid roleId, string keyword = null, string name = null, string phone = null);
    }
}
