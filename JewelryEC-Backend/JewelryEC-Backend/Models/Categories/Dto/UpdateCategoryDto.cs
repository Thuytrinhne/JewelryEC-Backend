using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Categories.Dto
{
    public class UpdateCategoryDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }
}
