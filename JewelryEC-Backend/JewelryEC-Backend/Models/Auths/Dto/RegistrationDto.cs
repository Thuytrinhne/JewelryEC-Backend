using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class RegistrationDto
    {
            [Required(ErrorMessage = "{0} is required.")]
            [EmailAddress(ErrorMessage = "{0} is not in correct format.")]
        public string Email { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(100, MinimumLength = 1,
            ErrorMessage = "Name should be minimum {2} characters and a maximum of {1} characters")]
            [Display(Name = "Customer Name")]
        public string Name { get; set; }

            [Phone(ErrorMessage = "{0} is not in correct format.")]   
        public string ? PhoneNumber { get; set; }

            [PasswordPropertyText]
        public string Password { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(6, ErrorMessage = "{0} should be {1} characters")]
        public string OTP {  get; set; }
    }
}
