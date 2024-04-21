using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Catalogs.Entities
{
    public class Catalog
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? ParentId { get; set; }
        public string? CatalogSlug { get; set; }
        public DateTime Created_at { get; set; }
    }
}
