using JewelryEC_Backend.Models.Auths.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace JewelryEC_Backend.Models.Carts.Entities
{
    public class Cart
    {
        [Key]
        public  Guid Id { get; set; }
        public int IsPayed { get; set; } = 0;
        public string ?UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;

    
    }
}
