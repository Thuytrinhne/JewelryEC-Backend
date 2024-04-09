using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JewelryEC_Backend.Models.Carts.Entities;

namespace JewelryEC_Backend.Models.CartItems.Dto
{
    public class CreateCartItemDto
    {
        public Guid ProductId { get; set; }
        [Required]
        public int Count { get; set; }
    }
}
