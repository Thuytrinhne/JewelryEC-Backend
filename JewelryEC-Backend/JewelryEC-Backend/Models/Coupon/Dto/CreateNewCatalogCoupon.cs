using System;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Coupon
{
    public class CreateCatalogProductCouponDto
    {
        [Required(ErrorMessage = "Discount value is required.")]
        public double DiscountValue { get; set; }

        [Required(ErrorMessage = "Discount unit is required.")]
        public int DiscountUnit { get; set; }

        [Required(ErrorMessage = "Created time is required.")]
        public DateTime CreatedTime { get; set; }

        [Required(ErrorMessage = "Valid from date is required.")]
        public DateTime ValidFrom { get; set; }

        [Required(ErrorMessage = "Valid to date is required.")]
        public DateTime ValidTo { get; set; }

        [Required(ErrorMessage = "Coupon code is required.")]
        public string CouponCode { get; set; }

        // Optional properties with nullable types
        public decimal? MinimumOrderValue { get; set; }

        public decimal? MaximumDiscountValue { get; set; }

        [Required(ErrorMessage = "Redeem allowed flag is required.")]
        public bool IsRedeemAllowed { get; set; }
    }
}
