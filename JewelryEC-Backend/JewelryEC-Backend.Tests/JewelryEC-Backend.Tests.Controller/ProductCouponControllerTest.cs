using AutoMapper;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models.Coupon.Dto;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace JewelryEC_Backend.Tests.Controller
{
    public class ProductCouponApiControllerTests
    {
        private readonly Mock<IProductCouponService> _mockService;
        private readonly ProductCouponApiController _productCouponController;
        private readonly IMapper _mapper;

        public ProductCouponApiControllerTests()
        {
            _mockService = new Mock<IProductCouponService>();
            _productCouponController = new ProductCouponApiController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_WhenProductCouponsExist_ReturnsOkResult()
        {
            // Arrange
            var productCoupons = new List<ProductCoupon>
            {
                new ProductCoupon { Id = Guid.NewGuid(), CouponCode = "Coupon1" },
            };
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(new SuccessDataResult<List<ProductCoupon>>(productCoupons));

            // Act
            var result = await _productCouponController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var responseDto = okResult.Value as ResponseDto;
            Assert.NotNull(responseDto);
            Assert.Equal(productCoupons, responseDto.Result);
        }

        [Fact]
        public async Task GetById_WhenProductCouponExists_ReturnsOkResult()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            var productCoupon = new ProductCoupon { Id = couponId, CouponCode = "Coupon1" };
            _mockService.Setup(s => s.GetById(couponId)).ReturnsAsync(new SuccessDataResult<ProductCoupon>(productCoupon));

            // Act
            var result = await _productCouponController.GetById(couponId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as SuccessDataResult<ProductCoupon>;
            Assert.NotNull(response);
            Assert.Equal(couponId, (response.Result as ProductCoupon).Id);
        }

        [Fact]
        public async Task Add_WhenProductCouponDtoIsValid_ReturnsOkResult()
        {
            // Arrange
            var productCouponDto = new CreateProductCouponDto { CouponCode = "NewCoupon" };
            var productCouponId = Guid.NewGuid();
            var response = new SuccessResult("Add productCoupon successfully", new ProductCoupon { Id = productCouponId, CouponCode = "NewCoupon" });

            _mockService.Setup(s => s.Add(productCouponDto)).ReturnsAsync(response);

            // Act
            var result = await _productCouponController.Add(productCouponDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var responseData = okResult.Value as SuccessResult;
            Assert.NotNull(responseData);
            var addedCoupon = responseData.Result as ProductCoupon;
            Assert.NotNull(addedCoupon);
            Assert.Equal(productCouponId, addedCoupon.Id);
        }

        [Fact]
        public async Task Update_WhenProductCouponDtoIsValid_ReturnsOkResult()
        {
            // Arrange
            var productCouponDto = new UpdateProductCouponDto { Id = Guid.NewGuid(), CouponCode = "UpdatedCoupon" };
            var response = new SuccessResult("Update productCoupon successfully");

            _mockService.Setup(s => s.Update(productCouponDto)).ReturnsAsync(response);

            // Act
            var result = await _productCouponController.Update(productCouponDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var responseData = okResult.Value as SuccessResult;
            Assert.NotNull(responseData);
            Assert.Equal("Update productCoupon successfully", responseData.Message);
        }

        [Fact]
        public async Task Delete_WhenProductCouponIdIsValid_ReturnsOkResult()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            _mockService.Setup(s => s.Delete(couponId)).ReturnsAsync(new SuccessResult());

            // Act
            var result = await _productCouponController.Delete(couponId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as SuccessResult;
            Assert.NotNull(response);
        }
    }
}
