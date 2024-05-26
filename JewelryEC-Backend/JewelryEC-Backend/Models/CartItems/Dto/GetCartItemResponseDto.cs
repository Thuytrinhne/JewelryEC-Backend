using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JewelryEC_Backend.Models.Carts.Entities;

namespace JewelryEC_Backend.Models.CartItems.Dto
{
    public class GetCartItemResponseDto
    {

        public Guid Id { get; set; }
        public Guid ProductItemId { get; set; }
        public int Count { get; set; }
        public string NameProduct { get; set; }
        public string Description { get; set; }
        public decimal   SalePrice { get; set; }
        public double DiscountPrice { get; set; }
        public string Image { get; set; }
        public Guid? UserCouponId { get; set; }


    }
}
