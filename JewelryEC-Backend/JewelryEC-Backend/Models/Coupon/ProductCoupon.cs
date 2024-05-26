﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Enum;

namespace JewelryEC_Backend.Models.Coupon
{
    public class ProductCoupon: IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Conditions { get; set; }

        public Guid ProductId { get; set; }

        public double DiscountValue { get; set; }

        public DiscountUnit DiscountUnit { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string CouponCode { get; set; }

        public decimal? MinimumOrderValue { get; set; }

        public decimal? MaximumDiscountValue { get; set; }

        public int UsedTime { get; set; }

        public bool IsRedeemAllowed { get; set; }

        public virtual Product Product { get; set; }
    }
}
