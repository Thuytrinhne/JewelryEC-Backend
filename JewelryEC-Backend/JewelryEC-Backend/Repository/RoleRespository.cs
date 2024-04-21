using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Roles.Entities;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryEC_Backend.Repository
{
    public class RoleRespository : GenericRepository<ApplicationRole>, IRoleRespository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleRespository(AppDbContext context, RoleManager<ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }


    }
}
