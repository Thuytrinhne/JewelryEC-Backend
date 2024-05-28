using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Auths.Entities;
using Microsoft.AspNetCore.Identity;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface IUserRespository : IGenericRepository<ApplicationUser>
    {
        Task<IdentityResult> AddUserByUserManager(ApplicationUser user, string password);
        Task<bool> Login(ApplicationUser user, string password);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(Guid roleId);
        Task<PaginationResult<ApplicationUser>> GetUsersByRoleAsyncPagination(PaginationRequest request, Guid roleId);

        Task<IEnumerable<string>> GetRoleAsync(ApplicationUser user);
        ApplicationUser GetUserById(Guid userid);
        Task<bool> AssignRoleForUser(ApplicationUser user, Guid roleId);
        Task<IdentityResult> ResetPassword(ApplicationUser user, string newPass);
        Task<ApplicationUser> UpdateUser(ApplicationUser user);
        Task<IdentityResult> ChangePassword(ApplicationUser user, string currentPass, string newPass);

        Task<PaginationResult<ApplicationUser>> SearchRecordsAsyncPagination(PaginationRequest request, Guid roleId, string keyword, string name = null, string phone = null);


    }
}
