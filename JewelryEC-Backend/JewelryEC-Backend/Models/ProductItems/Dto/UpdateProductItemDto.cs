using System;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Products.Dto
{
    public class UpdateProductItemDto
    {

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount price must be a non-negative value.")]
        public decimal? DiscountPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100.")]
        public float? DiscountPercent { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Tags cannot exceed 100 characters.")]
        public string? Tags { get; set; }

        [Url(ErrorMessage = "Image URL is not valid.")]
        public string? Image { get; set; }
    }
}
