using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;

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
                Description = productDto.Description,
                Code = productDto.Code,
                InternationalCode = productDto.InternationalCode,
                CatalogId = productDto.CatalogId,
                SaledCount = productDto.SaledCount,
                AverageRating = productDto.AverageRating,
                RatingCount = productDto.RatingCount,
                Availability = productDto.Availability,
            };
            newProduct.Items = productDto.Items.Select(itemDto => new ProductItem
            {
                Name = itemDto.Name,
                ProductSlug = itemDto.ProductSlug,
                SKU = itemDto.SKU,
                State = itemDto.State,
                UnitPrice = itemDto.UnitPrice,
                DiscountPrice = itemDto.DiscountPrice,
                DiscountPercent = itemDto.DiscountPercent,
                Stock = itemDto.Stock,
                ProductId = newProduct.Id
            }).ToList();
            newProduct.Images = productDto.Images.Select(imageDto => new ProductImage
            {
                ImageUrl = imageDto,
                ProductId = newProduct.Id
            }).ToList();
            return newProduct;
        }
        public static Product ProductFromUpdateProductDto(UpdateProductDto productDto)
        {
            Product newProduct = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Code = productDto.Code,
                InternationalCode = productDto.InternationalCode,
                CatalogId = productDto.CatalogId,
                SaledCount = productDto.SaledCount,
                AverageRating = productDto.AverageRating,
                RatingCount = productDto.RatingCount,
                Availability = productDto.Availability,
            };
            newProduct.Items = productDto.Items.Select(itemDto => new ProductItem
            {
                Name = itemDto.Name,
                ProductSlug = itemDto.ProductSlug,
                SKU = itemDto.SKU,
                State = itemDto.State,
                UnitPrice = itemDto.UnitPrice,
                DiscountPrice = itemDto.DiscountPrice,
                DiscountPercent = itemDto.DiscountPercent,
                Stock = itemDto.Stock,
                ProductId = newProduct.Id
            }).ToList();
            newProduct.Images = productDto.Images.Select(imageDto => new ProductImage
            {
                ImageUrl = imageDto,
                ProductId = newProduct.Id
            }).ToList();
            return newProduct;
        }

    }
}
