using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.CartItems.Dto;

namespace JewelryEC_Backend.Models.Carts.Dto
{
    public class CreateCartDto
    {
        public string? UserId { get; set; }
        public CreateCartItemDto ? CartItemDto { get; set; }
    }
}
