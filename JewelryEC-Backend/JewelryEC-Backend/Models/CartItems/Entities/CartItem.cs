using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using JewelryEC_Backend.Models.Carts.Entities;

namespace JewelryEC_Backend.Models.CartItems.Entities
{
    public class CartItem
    {

        [Key]
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        public Guid ProductId { get; set; }
        [Required]
        public int Count { get; set; }

    }
}
