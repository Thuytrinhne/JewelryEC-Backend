using JewelryEC_Backend.Models.Voucher;

namespace JewelryEC_Backend.Models.OrderItems
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Subtotal { get; set; }
        public Guid? UserCouponId { get; set; }
        public virtual UserCoupon UserCoupon { get; set; }

        //public Guid? CouponApplicationId { get; set; }
        //public virtual CouponApplication CouponApplication { get; set; }
    }
}
