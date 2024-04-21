using System.ComponentModel.DataAnnotations;
using static JewelryEC_Backend.Utility.SD;

namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId  { get; set; }
        
        [Required(ErrorMessage = "Role is required.")]
        public string ?RoleId { get; set; }
    }
}
