using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.OrderItems;

namespace JewelryEC_Backend.Models.Orders
{
    public class Order: IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
