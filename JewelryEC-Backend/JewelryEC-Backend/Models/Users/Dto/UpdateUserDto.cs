using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Users.Dto
{
    public class UpdateUserDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "{0} is not in correct format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Customer Name")]
        public string Name { get; set; }
        [Phone] 
        public string ? PhoneNumber { get; set; }
    }
}
