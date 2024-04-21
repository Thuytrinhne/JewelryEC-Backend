using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Roles.Entities;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using System.Reflection.Metadata.Ecma335;

namespace JewelryEC_Backend.Service
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateRole(ApplicationRole roleToCreate)
        {
            try
            {
                _unitOfWork.Roles.Add(roleToCreate);
                _unitOfWork.Save();
                  
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool DeleteRole(ApplicationRole roleToDelete)
        {
            try
            {
                _unitOfWork.Roles.Remove(roleToDelete);
                _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ApplicationRole GetRoleById(Guid id)
        {
           return  _unitOfWork.Roles.GetById(id);
        }

        public  IEnumerable<ApplicationRole> ListRoles()
        {
            return  _unitOfWork.Roles.GetAll();
        }

        public  bool UpdateRole(ApplicationRole roleToUpdate)
        {
            var roleEntity = GetRoleById(roleToUpdate.Id);
            if (roleEntity != null)
            {          
                if (!string.IsNullOrEmpty(roleToUpdate.Name))
                {
                    roleEntity.Name = roleToUpdate.Name;
                    _unitOfWork.Save();
                }  
                return true;
            }
            else return false;

        }
    }
}
