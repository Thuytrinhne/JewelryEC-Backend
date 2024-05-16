using JewelryEC_Backend.Models.Catalogs.Entities;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Products.Dto
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "CatalogId is required")]
        public Guid CatalogId { get; set; }

        [Required(ErrorMessage = "Average rating is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Average rating must be non-negative")]
        public float? AverageRating { get; set; }

        [Required(ErrorMessage = "Ratings count is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Ratings count must be non-negative")]
        public long? RatingCount { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one product item is required.")]
        public virtual ICollection<CreateProductItemDto> Items { get; set; }
    }
}
