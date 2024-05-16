using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Models.Catalogs.Entities;
using static System.Reflection.Metadata.BlobBuilder;

namespace JewelryEC_Backend.Models.Products
{
    public class Product: IEntity
    {
        public Product()
        {
            Items = new HashSet<ProductVariant>();
        }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid CatalogId { get; set; }
        public float? AverageRating { get; set; }
        public long? RatingCount { get; set; }
        public virtual ICollection<ProductVariant> Items { get; set; } 
        public virtual Catalog Catalog { get; set; }
        
    }
}
