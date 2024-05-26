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
        public Guid ProductItemId { get; set; }

            [Required(ErrorMessage = "{0} is required")]
        public int Count { get; set; }
        public Guid ? UserCouponId { get; set; }

    }
}
