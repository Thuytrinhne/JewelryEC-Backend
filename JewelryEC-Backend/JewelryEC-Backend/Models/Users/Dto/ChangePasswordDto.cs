using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Users.Dto
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }
    }
}
