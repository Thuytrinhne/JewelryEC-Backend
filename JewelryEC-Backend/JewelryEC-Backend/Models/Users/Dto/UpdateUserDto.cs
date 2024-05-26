using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Users.Dto
{
    public class UpdateUserDto
    {
     
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Customer Name")]
        public string Name { get; set; }
        [Phone] 
        public string ? PhoneNumber { get; set; }
    }
}
