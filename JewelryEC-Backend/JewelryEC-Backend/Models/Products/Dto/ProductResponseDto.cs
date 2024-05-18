using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Models.Products.Dto
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid CategoryId { get; set; }
        public float? AverageRating { get; set; }
        public long? RatingCount { get; set; }
        public virtual ICollection<ProductItemResponseDto> Items { get; set; }
        public virtual Catalog Catalog { get; set; }
    }
}
