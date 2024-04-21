using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "{0} is not in correct format.")]
        public string Email { get; set; }   
    }
}
