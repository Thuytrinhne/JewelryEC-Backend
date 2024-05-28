using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Users.Dto;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AssignRole(Guid userId, Guid roleId)
        {
            try
            {
                var user = _unitOfWork.Users.GetUserById(userId);
                if (user is not null)
                {
                    if (await _unitOfWork.Users.AssignRoleForUser(user, roleId))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while assigning role: " + ex.Message, ex);
            }

        }
        public ApplicationUser EditProfile(Guid UserId, UpdateUserDto user)
        {
            var userFrmDb = _unitOfWork.Users.GetUserById(UserId);
            if(!String.IsNullOrEmpty(user.Name))
                userFrmDb.Name = user.Name;

            if (!String.IsNullOrEmpty(user.PhoneNumber))
                userFrmDb.PhoneNumber = user.PhoneNumber;

            _unitOfWork.Users.Update(userFrmDb);
            _unitOfWork.Save();
            return userFrmDb;
        }

        public Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user)
        {
           return _unitOfWork.Users.GetRoleAsync(user);
        }

        public ApplicationUser GetUserById(Guid idUser)
        {
            return _unitOfWork.Users.GetUserById(idUser);
        }

        public async  Task<IEnumerable<ApplicationUser>> GetUsers()
        {    
                return _unitOfWork.Users.GetAll();
        }
        public async Task<PaginationResult<ApplicationUser> >GetUsersPagination(PaginationRequest request)
        {
            return await _unitOfWork.Users.GetAllPagination(request);
        }
        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleId(Guid roleId)
        {
            return await _unitOfWork.Users.GetUsersByRoleAsync(roleId);

        }

        public async Task<PaginationResult<ApplicationUser>> GetUsersByRoleIdPagination(Guid roleId, PaginationRequest request)
        {
            return await _unitOfWork.Users.GetUsersByRoleAsyncPagination(request, roleId);

        }

        public  async Task<bool> UpdatePassword(ApplicationUser user,string currentPass, string newPass )
        {
            if (user is not null)
            {

                var result = await _unitOfWork.Users.ChangePassword(user, currentPass, newPass);
                if (result.Succeeded)
                    return true;
            }
            return false;
        }
        public async Task<PaginationResult<ApplicationUser>> SearchRecordsAsyncPagination(PaginationRequest request,Guid roleId ,  string keyword=null, string name = null, string phone = null)
        {
            return await  _unitOfWork.Users.SearchRecordsAsyncPagination(request, roleId, keyword, name, phone);
        }


    }


  

}
