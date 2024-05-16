using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Deliveries.Dto;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.OrderItems.Dto;

namespace JewelryEC_Backend.Models.Orders.Dto
{
    public class CreateNewOrderFromCartDto
    {
        public Guid UserId { get; set; }
        public CreateDeliveryDto? DeliveryDto { get; set; }
        public Guid? DeliveryId { get; set;}
        public List<Guid> CartItemIds { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
