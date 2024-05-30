using JewelryEC_Backend.Controllers;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Orders.Dto;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.Helpers.Payments.VnPay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace JewelryEC_Backend.Tests.Controller
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<IVnPayService> _mockVnPayService;
        private readonly Mock<ICartService> _mockCartService;
        private readonly OrderApiController _orderController;

        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockVnPayService = new Mock<IVnPayService>();
            _mockCartService = new Mock<ICartService>();
            _orderController = new OrderApiController(_mockOrderService.Object, _mockVnPayService.Object, _mockCartService.Object);
        }

        [Fact]
        public async Task GetAll_WhenOrdersExist_ReturnsOkResult()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), TotalPrice = 100 }
            };
            _mockOrderService.Setup(s => s.GetAll(1, 10)).ReturnsAsync(new SuccessDataResult<List<Order>>(orders));

            // Act
            var result = await _orderController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as SuccessDataResult<List<Order>>;
            Assert.NotNull(response);
            Assert.Equal(orders, response.Result);
        }

        [Fact]
        public async Task GetById_WhenOrderExists_ReturnsOkResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, TotalPrice = 100 };
            _mockOrderService.Setup(s => s.GetById(orderId)).ReturnsAsync(new SuccessDataResult<Order>(order));

            // Act
            var result = await _orderController.GetById(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as SuccessDataResult<Order>;
            Assert.NotNull(response);
            Assert.Equal(orderId, (response.Result as Order).Id);
        }
        [Fact]
        public async Task Cancel_WhenOrderIdIsValid_ReturnsOkResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _mockOrderService.Setup(s => s.UpdateOrderStatus(orderId, OrderStatus.Cancelled))
                .ReturnsAsync(new SuccessResult());

            // Act
            var result = await _orderController.Cancel(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as SuccessResult;
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetByUserId_WhenOrdersExist_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), TotalPrice = 100 }
            };
            _mockOrderService.Setup(s => s.GetOrdersByUserId(userId, 1, 10))
                .ReturnsAsync(new SuccessDataResult<List<Order>>(orders));

            // Act
            var result = await _orderController.GetByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var response = okResult.Value as SuccessDataResult<List<Order>>;
            Assert.NotNull(response);
            Assert.Equal(orders, response.Result);
        }
    }
}
