using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Roles.Dto
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "{0} is required")]
        [DisplayName("Role Name")]
        public string? Name { get; set; }
    }
}
