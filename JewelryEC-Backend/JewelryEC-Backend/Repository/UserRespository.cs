using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Roles.Entities;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JewelryEC_Backend.Repository
{
    public class UserRespository : GenericRepository<ApplicationUser>, IUserRespository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRespository(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public  async Task<IdentityResult>  AddUsersByUserManager(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
                
        }

        public async Task<bool> AssignRoleForUser(ApplicationUser user,  string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return false;
            }
            // Thêm role cho người dùng
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
           
        }

        public async Task<IEnumerable<string>> GetRoleAsync(ApplicationUser user)
        {
             return  await _userManager.GetRolesAsync(user);
        }

        public ApplicationUser GetUserById(Guid userid)
        {
            return _context.ApplicationUsers.FirstOrDefault(u => u.Id == userid);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool>  Login (ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);

        }

        public async Task<IdentityResult> ResetPassword(ApplicationUser user, string newPass)
        {
            // Thực hiện cập nhật mật khẩu
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPass);
            return result;
        }

        public async  Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return null;
            }
            return await _userManager.GetUsersInRoleAsync(role.Name);  
        }

        public async  Task<IdentityResult> AddUserByUserManager(ApplicationUser user, string password)
        {
            return  await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser> UpdateUser(ApplicationUser user)
        {
            var originalUser = _context.Users.FirstOrDefault(u=>u.Id == user.Id);
            _context.Entry(originalUser).CurrentValues.SetValues(user);

            return user; 
        }
    }
}
