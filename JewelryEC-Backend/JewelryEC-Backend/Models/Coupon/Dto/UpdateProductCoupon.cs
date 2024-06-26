﻿using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Coupon.Dto
{
    public class UpdateProductCouponDto
    {
        [Required(ErrorMessage = "Coupon ID is required.")]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Conditions { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public Guid ProductId { get; set; }

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
