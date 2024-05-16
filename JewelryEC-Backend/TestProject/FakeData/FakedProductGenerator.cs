using Bogus;
using Bogus.DataSets;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.FakeData
{
    public static class FakeDataGenerator
    {
        public static List<Product> GetFakeProducts(int count)
        {
            var faker = new Faker<Product>()
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Code, f => f.Commerce.Ean8())
                .RuleFor(p => p.InternationalCode, f => f.Commerce.Ean13())
                .RuleFor(p => p.CatalogId, f => f.Random.Guid())
                .RuleFor(p => p.AverageRating, f => f.Random.Float(1, 5))
                .RuleFor(p => p.RatingCount, f => f.Random.Long(0, 100))
                .RuleFor(p => p.Created_at, f => f.Date.Past())
                .RuleFor(p => p.Updated_at, f => f.Date.Recent())
                .FinishWith((f, p) =>
                {
                    p.Items = GetFakeProductItems(f.Random.Int(1, 5), p.Id);
                });

            return faker.Generate(count);
        }

        private static List<ProductItem> GetFakeProductItems(int count, Guid productId)
        {
            var faker = new Faker<ProductItem>()
                .RuleFor(pi => pi.Id, f => f.Random.Guid())
                .RuleFor(pi => pi.Size, f => "Normal")
                .RuleFor(pi => pi.ProductSlug, f => f.Lorem.Word())
                .RuleFor(pi => pi.SKU, f => f.Commerce.Ean13())
                .RuleFor(pi => pi.UnitPrice, f => f.Random.Decimal(10, 1000))
                .RuleFor(pi => pi.DiscountPrice, f => f.Random.Decimal(5, 500))
                .RuleFor(pi => pi.DiscountPercent, f => f.Random.Int(0, 50))
                .RuleFor(pi => pi.Stock, f => f.Random.Int(0, 100))
                .RuleFor(pi => pi.State, f => f.PickRandom("Available", "Unavailable"))
                .RuleFor(pi => pi.ProductId, productId);

            return faker.Generate(count);
        }
        public static CreateProductDto GenerateFakeProductDto()
        {
            var faker = new Faker();

            var productDto = new CreateProductDto
            {
                Name = faker.Commerce.ProductName(),
                CatalogId = Guid.NewGuid(),
                Description = faker.Commerce.ProductDescription(),
                Code = faker.Commerce.Ean13(),
                InternationalCode = faker.Commerce.Ean8(),
                SaledCount = faker.Random.Long(0, 1000),
                AverageRating = (float)faker.Random.Double(0, 5),
                RatingCount = faker.Random.Long(0, 1000),
                Availability = faker.Random.Double(0, 100),
                Items = GenerateFakeProductItemDtos(),
                Images = GenerateFakeImages()
            };

            return productDto;
        }

        private static List<CreateProductItemDto> GenerateFakeProductItemDtos()
        {
            var faker = new Faker();

            var productItemDtos = new List<CreateProductItemDto>();
            for (int i = 0; i < 3; i++) // Generate 3 product items
            {
                var productItemDto = new CreateProductItemDto
                {
                    ProductSlug = faker.Commerce.ProductName(),
                    SKU = faker.Random.AlphaNumeric(10),
                    UnitPrice = faker.Random.Decimal(10, 100),
                    DiscountPrice = faker.Random.Decimal(5, 50),
                    DiscountPercent = faker.Random.Number(5, 50),
                    Stock = faker.Random.Int(0, 100),
                    State = faker.Commerce.ProductAdjective()
                };
                productItemDtos.Add(productItemDto);
            }

            return productItemDtos;
        }

        private static List<string> GenerateFakeImages()
        {
            var faker = new Faker();

            var images = new List<string>();
            for (int i = 0; i < 3; i++) // Generate 3 fake image URLs
            {
                images.Add(faker.Image.LoremFlickrUrl());
            }

            return images;
        }
    }

}
}
