using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Catalogs.Dto
{
    public class GetCatalogResponseDto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
        public Guid? ParentId { get; set; }
        public string? CatalogSlug { get; set; }
        public string Description { get; set; } = default!;
        public string Image { get; set; } = default!;
        public int State { get; set; } = 1;

    }
}
