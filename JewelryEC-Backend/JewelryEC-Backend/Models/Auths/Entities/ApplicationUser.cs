using Microsoft.AspNetCore.Identity;

namespace JewelryEC_Backend.Models.Auths.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string ? Name { get; set; }
    }
}
