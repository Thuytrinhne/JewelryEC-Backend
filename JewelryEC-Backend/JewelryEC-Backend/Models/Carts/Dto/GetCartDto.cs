using JewelryEC_Backend.Models.Auths.Entities;
using System.ComponentModel.DataAnnotations;

namespace JewelryEC_Backend.Models.Carts.Dto
{
    public class GetCartDto
    {

        public Guid Id { get; set; }
        public int IsPayed { get; set; } = 0;
        public string? UserId { get; set; }


    }
}
