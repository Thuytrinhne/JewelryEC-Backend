namespace JewelryEC_Backend.Models.Coupon.Dto
{
    public class CreateProductCouponDto
    {
        public Guid ProductId { get; set; }

        public double DiscountValue { get; set; }

        public int DiscountUnit { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string CouponCode { get; set; }

        public decimal? MinimumOrderValue { get; set; }

        public decimal? MaximumDiscountValue { get; set; }

        public bool IsRedeemAllowed { get; set; }
    }
}
