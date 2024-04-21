using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Categories.Dto
{
    public class CreateNewCategoryDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
    }
}
