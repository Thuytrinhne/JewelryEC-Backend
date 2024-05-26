using System;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.OrderItems.Dto
{
    public class CreateOrderItemDto
    {
        [Required(ErrorMessage = "Product item ID is required.")]
        public Guid ProductItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        public Guid? UserCouponId { get; set; }
    }
}
