using JewelryEC_Backend.Models.Carts.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace JewelryEC_Backend.Models.Auths.Entities
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string ? Name { get; set; }

    }
}
