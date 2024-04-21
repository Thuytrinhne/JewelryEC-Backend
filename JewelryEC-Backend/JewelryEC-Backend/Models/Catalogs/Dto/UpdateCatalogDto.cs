using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Catalogs.Dto
{
    public class UpdateCatalogDto
    {
        [Key]
        public Guid Id { get; set; }
  
            [Required(ErrorMessage = "{0} is required")]
            [DisplayName("Catalog Name")]
        public string? Name { get; set; }

        public Guid? ParentId { get; set; } = null;
        public string? CatalogSlug { get; set; } = null;

    }
}
