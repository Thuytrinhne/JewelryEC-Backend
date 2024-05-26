using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Users.Dto;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;

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

        public async  Task<IEnumerable<ApplicationUser>> ListUsers(Guid roleId )
        {
            if(roleId == Guid.Empty)
                return _unitOfWork.Users.GetAll();
            return await _unitOfWork.Users.GetUsersByRoleAsync(roleId);
        }
    }
}
