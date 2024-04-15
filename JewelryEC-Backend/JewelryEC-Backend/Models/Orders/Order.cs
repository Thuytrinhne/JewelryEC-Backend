using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.OrderItems;

namespace JewelryEC_Backend.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public long AddressId { get; set; }
        public OrderStatus orderStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
