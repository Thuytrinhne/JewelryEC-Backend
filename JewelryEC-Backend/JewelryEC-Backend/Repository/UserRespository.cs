using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Roles.Entities;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        public async Task<bool> AssignRoleForUser(ApplicationUser user,  Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false;
            }
            // Thêm role cho người dùng
            try
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name.Trim());
                if (!result.Succeeded)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
           
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
        public async Task <IdentityResult> ChangePassword (ApplicationUser user, string currentPass, string newPass)
        {
            return  await _userManager.ChangePasswordAsync(user, currentPass, newPass);

        }

        public async  Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return null;
            }
            return await _userManager.GetUsersInRoleAsync(role.Name);  
        }

        public async Task<PaginationResult <ApplicationUser>> GetUsersByRoleAsyncPagination (PaginationRequest request, Guid roleId)
        {
            var totalCount = await _context.Catalogs.LongCountAsync();

            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return null;
            }
            var usersInRole=  await _userManager.GetUsersInRoleAsync(role.Name);
            var pagedUsers = usersInRole.Skip(request.PageSize * request.PageIndex)
                               .Take(request.PageSize)
                               .ToList();

            return new PaginationResult<ApplicationUser>(
                  request.PageIndex,
                  request.PageSize,
                  totalCount,
                  pagedUsers);
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
        public async Task<PaginationResult<ApplicationUser>> SearchRecordsAsyncPagination(PaginationRequest request, Guid roleId = default!, string keyword = null, string name = null, string phone = null)
        {
            // Bắt đầu từ một biểu thức đúng với tất cả các phần tử
            Expression<Func<ApplicationUser, bool>> userExpression = user => true;

            if (!string.IsNullOrEmpty(keyword))
            {
                userExpression = userExpression.And(user =>
                    EF.Functions.ILike(user.Name, $"%{keyword}%") ||
                    EF.Functions.ILike(user.Email, $"%{keyword}%") ||
                    EF.Functions.ILike(user.PhoneNumber, $"%{keyword}%"));
            }

            if (!string.IsNullOrEmpty(name))
            {
                userExpression = userExpression.And(user => EF.Functions.ILike(user.Name, $"%{name}%"));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                userExpression = userExpression.And(user => EF.Functions.ILike(user.PhoneNumber, $"%{phone}%"));
            }


            // Tạo truy vấn từ biểu thức tìm kiếm
            var query = _context.ApplicationUsers
                                .Where(userExpression);

            ////// Nếu có điều kiện cho bảng Role
            //if (roleId.HasValue)
            //{
            //    query = query.Where(user => user.UserRoles.Any(ur => ur.RoleId == roleId.Value));
            //}

            // Thêm sắp xếp
            if (!string.IsNullOrEmpty(name))
            {
                query = query.OrderBy(user => user.Name);
            }
            else if (!string.IsNullOrEmpty(phone))
            {
                query = query.OrderBy(user => user.PhoneNumber);
            }
            else
            {
                query = query.OrderBy(user => user.Id); // Hoặc một trường mặc định nào đó
            }

            var totalCount = await query.CountAsync();

            // Áp dụng phân trang
            var users = await query.Skip(request.PageSize * request.PageIndex)
                                   .Take(request.PageSize)
                                   .ToListAsync();

            return new PaginationResult<ApplicationUser>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                users);
        }

    }
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var body = Expression.AndAlso(
                Expression.Invoke(expr1, parameter),
                Expression.Invoke(expr2, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
