namespace JewelryEC_Backend.Models.Products.Dto
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string description { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
