namespace JewelryEC_Backend.Models.OrderItems.Dto
{
    public class CreateOrderItemDto
    {
        public Guid ProductItemId { get; set; }
        public int Quantity { get; set; }
    }
}
