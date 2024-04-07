using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Products.Dto
{
    public class CreateNewCategory
    {
        [Required]
        public string? Name { get; set; }
    }
}
