using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Deliveries.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Add this namespace

namespace JewelryEC_Backend.Models.Orders.Dto
{
    public class CreateNewOrderFromCartDto
    {
        //public Guid UserId { get; set; }

        //[Required(ErrorMessage = "Delivery details are required.")]
        public CreateDeliveryDto? DeliveryDto { get; set; }

        //[Required(ErrorMessage = "Delivery ID is required.")]
        public Guid? DeliveryId { get; set; }

        [Required(ErrorMessage = "Cart item IDs are required.")]
        [MinLength(1, ErrorMessage = "At least one cart item ID is required.")]
        public List<Guid> CartItemIds { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
