namespace JewelryEC_Backend.Models.Products.Dto
{
    public class CreateProductItemDto
    {
        public string Name { get; set; }
        public string ProductSlug { get; set; }
        public string SKU { get; set; }
        public string State { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int DiscountPercent { get; set; }
        public int Stock { get; set; }
        public Guid ProductId { get; set; }
    }
}
