using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Models.Products.Dto
{
    public class UpdateProductDto
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
        public virtual ICollection<ProductItem> Items { get; set; } 
        public virtual ICollection<String> Images { get; set; }
    }
}
