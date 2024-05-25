using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.Orders;

namespace JewelryEC_Backend.Models.Voucher
{
    public class CouponApplication: IEntity
    {

        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }
        public Guid UserCouponId { get; set; }
        public decimal DiscountAmount;
        public virtual OrderItem OrderItem { get; set; }
        public virtual UserCoupon UserCoupon { get; set; }
    }
}
