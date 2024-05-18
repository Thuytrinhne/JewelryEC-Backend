using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Models.Products.Dto
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid CatalogId { get; set; }
        public float? AverageRating { get; set; }
        public long? RatingCount { get; set; }
        public virtual ICollection<UpdateProductItemDto> Items { get; set; } 
    }
}
