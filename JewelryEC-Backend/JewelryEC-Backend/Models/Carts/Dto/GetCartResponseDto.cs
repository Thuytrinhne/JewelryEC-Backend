using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.CartItems.Dto;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace JewelryEC_Backend.Models.Carts.Dto
{
    public class GetCartResponseDto
    {
        public int IsPayed { get; set; } = 0;
        public string? UserId { get; set; }
        public List<GetCartItemResponseDto> ? Items { get; set; }
        public decimal? TotalPrice { get; set; } = 0;

        
    }
}
