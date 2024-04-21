using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Models.Products;

namespace JewelryEC_Backend.Models.Categories
{
    public class Category: IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        ICollection<Product> Products { get; set; }
        public bool IsActive;
    }
}
