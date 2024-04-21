using FluentValidation.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Catalogs.Dto
{
    public class CreateCatalogDto
    {
            [Required(ErrorMessage = "{0} is required")]
            [DisplayName("Catalog Name")]    
        public string Name { get; set; }

        public Guid? ParentId { get; set; } = null;
        public string? CatalogSlug { get; set; } = null;

    }
}
