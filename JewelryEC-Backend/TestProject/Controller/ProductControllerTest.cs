using Xunit;
using JewelryEC_Backend.Controllers;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.Models.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Data;
using Microsoft.EntityFrameworkCore;
using TestProject.FakeData;
using TestProject.Utils;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Repository;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.UnitOfWork;
using JewelryEC_Backend.Core.Utilities.Results;
using System.Web.Http.Results;

namespace TestProject.Controller
{
    public class ProductControllerTest
    {
        
        private readonly Mock<AppDbContext> _dbContextMock;
        private List<Product> _fakeProducts = FakeDataGenerator.GetFakeProducts(3);
        private Mock<UnitOfWork> unitOfWork;
        private ProductApiController _productController;
        private ProductService _productService;

        public ProductControllerTest()
        {
            _dbContextMock = new Mock<AppDbContext>();
            _dbContextMock.Setup(x => x.Products).Returns(UnitTestHelpers.GetQueryableMockDbSet(_fakeProducts));
            unitOfWork = new Mock<UnitOfWork>(_dbContextMock.Object, null, null);
            unitOfWork.Setup(x => x.Products).Returns(new ProductRepository(_dbContextMock.Object));
            _productService = new ProductService(unitOfWork.Object);
            _productController = new ProductApiController(_productService);
        }
        [Fact]
        public async Task GetAll_Returns_Ok_WithProducts()
        {
            var actionResult = await _productController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var resultProducts = Assert.IsType<SuccessDataResult< List<Product>>>(okResult.Value);
            Assert.Equal(_fakeProducts, resultProducts.Result);
        }
        [Fact]
        public async Task GetById_Returns_Ok_WithProduct()
        {
            // Arrange
            var productId = _fakeProducts[0].Id;
            // Act
            var actionResult = await _productController.GetById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var resultProduct = Assert.IsType<SuccessDataResult<Product>>(okResult.Value);
            Assert.Equal(_fakeProducts[0], resultProduct.Result);
        }
        [Fact]
        public async Task Add_Returns_Ok()
        {
            var productDto = FakeDataGenerator.GenerateFakeProductDto();

            var actionResult = await _productController.Add(productDto);
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var resultProduct = Assert.IsType<SuccessDataResult<Product>>(okResult.Value);
            Assert.Equal(_fakeProducts[0], resultProduct.Result);

        }


    }
}
