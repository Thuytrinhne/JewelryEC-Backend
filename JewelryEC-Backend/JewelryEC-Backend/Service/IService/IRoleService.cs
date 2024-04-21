using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Roles.Entities;

namespace JewelryEC_Backend.Service.IService
{
    public interface IRoleService
    {
        bool CreateRole(ApplicationRole roleToCreate);
        IEnumerable<ApplicationRole> ListRoles();
        ApplicationRole GetRoleById(Guid id);
        bool UpdateRole(ApplicationRole roleToUpdate);
        bool DeleteRole(ApplicationRole roleToDelete);
    }
}
