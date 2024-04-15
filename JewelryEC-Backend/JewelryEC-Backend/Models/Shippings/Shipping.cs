using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Deliveries;

namespace JewelryEC_Backend.Models.Shippings
{
    public class Shipping: IEntity
    {
        public Guid Id { get; set; }
        public DateTime expectationShippingDate { get; set; }
        public DateTime actualShippingDate { get; set; }
        public Guid DeliveryId { get; set; }
        public ShippingStatus ShippingStatus { get; set; }
        public Guid OrderId { get; set; }
        public virtual Delivery Delivery { get; set; }
    }
}
