namespace JewelryEC_Backend.Models.CartItems.Dto
{
    public class CreateCartItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductItemId { get; set; }
        public int Count { get; set; }
    }
}
