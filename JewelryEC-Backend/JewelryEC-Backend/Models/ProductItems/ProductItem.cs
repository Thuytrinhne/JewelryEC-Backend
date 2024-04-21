using JewelryEC_Backend.Core.Entity;

namespace JewelryEC_Backend.Models.Products
{
    public class ProductItem: IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProductSlug { get; set; }
        public string SKU { get; set; }
        public string State { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int DiscountPercent { get; set; }    
        public int Stock { get; set;}
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
