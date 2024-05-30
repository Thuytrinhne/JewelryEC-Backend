using AutoMapper;
using JewelryEC_Backend.Controllers;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models.Coupon.Dto;
using JewelryEC_Backend.Models.Voucher;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace JewelryEC_Backend.Tests.Controller
{
    public class UserCouponApiControllerTests
    {
        private readonly Mock<IUserCouponService> _mockService;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly UserCouponApiController _userCouponController;

        public UserCouponApiControllerTests()
        {
            _mockService = new Mock<IUserCouponService>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _userCouponController = new UserCouponApiController(_mockService.Object, _mockHttpContextAccessor.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            }, "mock"));

            _mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(new DefaultHttpContext { User = user });
        }

        [Fact]
        public async Task GetByCouponStatus_WhenCouponsExist_ReturnsOkResult()
        {
            // Arrange
            var userId = _mockHttpContextAccessor.Object.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var status = CouponStatus.ACTIVE;
            var userCoupons = new List<UserCoupon>
            {
                new UserCoupon { Id = Guid.NewGuid(), Status = CouponStatus.ACTIVE, UserId = new Guid(userId) },
            };
            _mockService.Setup(s => s.FindCouponByStatusAndUserId(status, new Guid(userId)))
                .ReturnsAsync(new SuccessDataResult<List<UserCoupon>>(userCoupons));

            // Act
            var result = await _userCouponController.GetByCouponStatus(status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var responseDto = okResult.Value as ResponseDto;
            Assert.NotNull(responseDto);
            Assert.Equal(userCoupons, responseDto.Result);
        }

        [Fact]
        public async Task GetById_WhenCouponExists_ReturnsOkResult()
        {
            // Arrange
            var couponId = Guid.NewGuid();
            var userCoupon = new UserCoupon { Id = couponId };
            _mockService.Setup(s => s.ReceiveCoupon(null, couponId))
                .ReturnsAsync(new SuccessDataResult<UserCoupon>(userCoupon));

            // Act
            var result = await _userCouponController.GetById(null, couponId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as SuccessDataResult<UserCoupon>;
            Assert.NotNull(response);
            Assert.Equal(couponId, (response.Result as UserCoupon).Id);
        }

        [Fact]
        public async Task TryApplyCoupon_WhenApplicationIsValid_ReturnsOkResult()
        {
            // Arrange
            var userCouponId = Guid.NewGuid();
            var productItemId = Guid.NewGuid();
            var userCoupon = new UserCoupon { Id = userCouponId };
            _mockService.Setup(s => s.tryApplyCoupon(userCouponId, productItemId))
                .ReturnsAsync(new SuccessDataResult<UserCoupon>(userCoupon));

            // Act
            var result = await _userCouponController.TryApplyCoupon(userCouponId, productItemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as SuccessDataResult<UserCoupon>;
            Assert.NotNull(response);
            Assert.Equal(userCouponId, (response.Result as UserCoupon).Id);
        }
    }
}
