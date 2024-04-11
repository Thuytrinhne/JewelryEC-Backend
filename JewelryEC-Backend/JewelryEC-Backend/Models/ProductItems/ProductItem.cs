namespace JewelryEC_Backend.Models.Products
{
    public class ProductItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProductSlug { get; set; }
        public string SKU { get; set; }
        public string State { get; set; }
        public string ImageU{ get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int TradeMark { get; set; }
        public string Stock { get; set;}
        public Guid ProductId { get; set; }
      
    }
}
