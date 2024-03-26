using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Catalogs.Dto
{
    public class UpdateCatalogDto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public Guid? ParentId { get; set; } = null;
        public string? CatalogSlug { get; set; } = null;

    }
}
