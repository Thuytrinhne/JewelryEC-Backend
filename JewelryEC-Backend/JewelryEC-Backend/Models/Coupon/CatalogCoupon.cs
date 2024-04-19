using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Models.Coupon
{
    public class CatalogCoupon: IEntity
    {
        public Guid Id { get; set; }

        public Guid CatalogId { get; set; }

        public double DiscountValue { get; set; }

        public int DiscountUnit { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string CouponCode { get; set; }

        public decimal? MinimumOrderValue { get; set; }

        public decimal? MaximumDiscountValue { get; set; }

        public bool IsRedeemAllowed { get; set; }

        public virtual Catalog Catalog { get; set; }
    }
}
