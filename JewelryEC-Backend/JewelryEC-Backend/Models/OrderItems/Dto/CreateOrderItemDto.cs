namespace JewelryEC_Backend.Models.OrderItems.Dto
{
    public class CreateOrderItemDto
    {
        public Guid ProductItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
