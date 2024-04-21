using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; }
        
    }
}
