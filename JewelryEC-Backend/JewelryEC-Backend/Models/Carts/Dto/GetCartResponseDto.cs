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
        public float? TotalPrice { get; set; }
        public GetCartResponseDto()
        {
            // Khởi tạo TotalPrice là 0
            TotalPrice = 0;

            // Nếu Items không null và có ít nhất một mục
            if (Items != null && Items.Any())
            {
                // Tính tổng giá của các mục trong Items
                TotalPrice = Items.Sum(item => item.SalePrice);
            }
        }
    }
}
