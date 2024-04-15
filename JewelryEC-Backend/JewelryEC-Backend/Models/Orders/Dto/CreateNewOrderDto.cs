using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Deliveries.Dto;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.OrderItems.Dto;

namespace JewelryEC_Backend.Models.Orders.Dto
{
    public class CreateNewOrderDto
    {
        public Guid UserId { get; set; }
        public CreateDeliveryDto? DeliveryDto { get; set; }
        public Guid? DeliveryId { get; set;}
        public virtual ICollection<CreateOrderItemDto> OrderItems { get; set; }
    }
}
