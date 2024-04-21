using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Entities;
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

        public ApplicationUser EditProfile(ApplicationUser user)
        {
            _unitOfWork.Users.UpdateUser(user);
            _unitOfWork.Save();
            return user;
        }

        public Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user)
        {
           return _unitOfWork.Users.GetRoleAsync(user);
        }

        public ApplicationUser GetUserById(Guid idUser)
        {
            return _unitOfWork.Users.GetUserById(idUser);
        }

        public async  Task<IEnumerable<ApplicationUser>> ListUsers(string roleId = "")
        {
            if(string.IsNullOrEmpty(roleId))
                return _unitOfWork.Users.GetAll();
            return await _unitOfWork.Users.GetUsersByRoleAsync(roleId);
        }
        
    }
}
