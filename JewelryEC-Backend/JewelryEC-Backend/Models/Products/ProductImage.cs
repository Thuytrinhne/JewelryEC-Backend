using JewelryEC_Backend.Core.Entity;

namespace JewelryEC_Backend.Models.Products
{
    public class ProductImage: IEntity
    {
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
