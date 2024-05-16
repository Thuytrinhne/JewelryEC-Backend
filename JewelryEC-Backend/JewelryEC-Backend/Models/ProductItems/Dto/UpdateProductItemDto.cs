namespace JewelryEC_Backend.Models.Products.Dto
{
    public class UpdateProductItemDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public float? DiscountPercent { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public string? Image { get; set; }
    }
}
