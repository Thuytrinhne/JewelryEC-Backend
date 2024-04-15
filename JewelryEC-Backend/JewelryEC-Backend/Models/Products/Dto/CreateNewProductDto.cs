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

        [Required(ErrorMessage = "Description is required")]
        [MinLength(1, ErrorMessage = "Description length must be greater than one")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string? Code { get; set; }

        public string? InternationalCode { get; set; }

        [Required(ErrorMessage = "Sale count is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Sale count must be non-negative")]
        public long? SaledCount { get; set; }

        [Required(ErrorMessage = "Average rating is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Average rating must be non-negative")]
        public float? AverageRating { get; set; }

        [Required(ErrorMessage = "Ratings count is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Ratings count must be non-negative")]
        public long? RatingCount { get; set; }

        [Required(ErrorMessage = "Availability is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Availability must be non-negative")]
        public double Availability { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one product item is required.")]
        public virtual ICollection<CreateProductItemDto> Items { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one genre is required.")]
        public virtual ICollection<String> Images { get; set; }

    }
}
