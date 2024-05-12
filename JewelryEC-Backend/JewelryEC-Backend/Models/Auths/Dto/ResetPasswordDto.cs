using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage ="{0} is required")]
        [PasswordPropertyText]
        public string NewPassword { get; set; }
        
    }
}
