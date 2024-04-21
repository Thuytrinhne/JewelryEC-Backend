using JewelryEC_Backend.Models.Auths.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static JewelryEC_Backend.Utility.SD;

namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        public Guid UserId  { get; set; }
        
        [Required(ErrorMessage = "{0} is required.")]
        public string RoleId { get; set; }

  
    
    }
}
