using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelryEC_Backend.Mapper
{
    public class ProductMapper
    {
        public static Product ProductFromCreateProductDto(CreateProductDto productDto)
        {
            Product newProduct = new Product
            {
                Id = new Guid(),
                Name = productDto.Name,
                CatalogId = productDto.CatalogId,
                AverageRating = productDto.AverageRating,
                RatingCount = productDto.RatingCount,
            };
            newProduct.Items = productDto.Items.Select(itemDto => new ProductVariant
            {
                DiscountPrice = itemDto.DiscountPrice,
                DiscountPercent = itemDto.DiscountPercent,
                ProductId = newProduct.Id,
                Price = itemDto.Price,
                Description = itemDto.Description ?? "",
                Tags = itemDto.Tags ?? "",
                Image = itemDto.Image ?? ""

            }).ToList();
            return newProduct;
        }
        public static Product ProductFromUpdateProductDto(UpdateProductDto productDto)
        {
            Product newProduct = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                CatalogId = productDto.CatalogId,
                AverageRating = productDto.AverageRating,
                RatingCount = productDto.RatingCount,

            };
            newProduct.Items = productDto.Items.Select(itemDto => new ProductVariant
            {
                Id = itemDto.Id,    
                DiscountPrice = itemDto.DiscountPrice,
                DiscountPercent = itemDto.DiscountPercent,
                ProductId = newProduct.Id,
                Price = itemDto.Price,
                Description = itemDto.Description,
                Tags = itemDto.Tags,
                Image = itemDto.Image
                }).ToList();
            return newProduct;
        }

    }
}
