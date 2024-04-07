namespace JewelryEC_Backend.Models.Products.Dto
{
    public class CreateNewProduct
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
