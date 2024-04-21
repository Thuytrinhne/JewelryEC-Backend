using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Validations.CustomAttributes;
using JewelryEC_Backend.UnitOfWork;

namespace JewelryEC_Backend.Models.CartItems.Dto
{
    public class CreateCartItemDto
    {
            [Required(ErrorMessage = "{0} is required")]
           // [ProductIdExists(ErrorMessage = "Should not be null or empty.")]
        public Guid ProductId { get; set; }

            [Required(ErrorMessage = "{0} is required")]
            [Range(1,500, ErrorMessage= "Please enter correct value [{1},{2}]")]
        public int Count { get; set; }
    }
}
