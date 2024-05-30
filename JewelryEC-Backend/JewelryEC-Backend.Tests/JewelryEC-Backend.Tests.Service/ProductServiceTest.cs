// ProductServiceTests.cs
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JewelryEC_Backend.Core.Filter;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.UnitOfWork;
using Moq;
using Xunit;

namespace JewelryEC_Backend.Tests.Service
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

            _productService = new ProductService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAll_WithValidPageNumberAndPageSize_ReturnsSuccessDataResult()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1" },
                new Product { Id = Guid.NewGuid(), Name = "Product 2" },
            };

            _mockProductRepository.Setup(repo => repo.GetProducts(pageNumber, pageSize, null))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAll(pageNumber, pageSize);

            // Assert
            var successResult = Assert.IsType<SuccessDataResult<List<Product>>>(result);
            Assert.Equal(products, successResult.Result);
        }




        [Fact]
        public async Task Delete_WithValidProductId_ReturnsSuccessResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };

            _mockProductRepository.Setup(repo => repo.GetProduct(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);
            _mockProductRepository.Setup(repo => repo.Delete(product))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _productService.Delete(productId);

            // Assert
            var successResult = Assert.IsType<SuccessResult>(result);
        }


        [Fact]
        public async Task GetById_WithValidProductId_ReturnsSuccessDataResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };

            _mockProductRepository.Setup(repo => repo.GetProduct(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetById(productId);

            // Assert
            var successResult = Assert.IsType<SuccessDataResult<Product>>(result);
            Assert.Equal(product, successResult.Result);
        }
    }
}
