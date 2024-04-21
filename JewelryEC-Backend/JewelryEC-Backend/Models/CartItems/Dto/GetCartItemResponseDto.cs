using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JewelryEC_Backend.Models.Carts.Entities;

namespace JewelryEC_Backend.Models.CartItems.Dto
{
    public class GetCartItemResponseDto
    {

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public string NameProduct { get; set; }
        public string DescriptionProduct { get; set; }
        public float  SalePrice { get; set; }
    }
}
