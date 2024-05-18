using JewelryEC_Backend.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelryEC_Backend.Models.Products
{
    public class ProductVariant: IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public double? DiscountPercent { get; set; }
        public string Image { get; set; }
        [ForeignKey("Product")]
        public  Guid  ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
