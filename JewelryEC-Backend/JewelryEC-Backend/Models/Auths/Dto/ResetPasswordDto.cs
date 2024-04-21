using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage ="{0} is required")]
        public string NewPassword { get; set; }
        
    }
}
