using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.Shippings;

namespace JewelryEC_Backend.Models.Orders
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.COD;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual Shipping Shipping { get; set; }

    }
}
