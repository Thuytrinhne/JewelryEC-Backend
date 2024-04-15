using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Models.Products
{
    public class Product: IEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public string? Code { get; set; }
        public string? InternationalCode { get; set; }
        public Guid CatalogId { get; set; }
        public long? SaledCount { get; set; }
        public float? AverageRating { get; set; }
        public long? RatingCount { get; set; }
        public double Availability { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public virtual ICollection<ProductItem> Items { get; set; } = new List<ProductItem>();
        public virtual Catalog Catalog { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
