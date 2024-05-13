using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Catalogs.Entities
{
    public class Catalog
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Guid? ParentId { get; set; }
        public string? CatalogSlug { get; set; }
        public string Image { get; set; } = default!;
        public int State { get; set; } = 1;
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
