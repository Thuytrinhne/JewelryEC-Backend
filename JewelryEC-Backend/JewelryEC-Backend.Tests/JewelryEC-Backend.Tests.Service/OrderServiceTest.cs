using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Deliveries.Dto;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.OrderItems.Dto;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Orders.Dto;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Shippings;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace JewelryEC_Backend.Tests.Service
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<ICartItemRepository> _mockCartItemRepository;
        private readonly Mock<IShippingRepository> _mockShippingRepository;
        private readonly Mock<IProductItemRespository> _mockProductItemRepository;
        private readonly Mock<IUserCouponRepository> _mockUserCouponRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockCartItemRepository = new Mock<ICartItemRepository>();
            _mockShippingRepository = new Mock<IShippingRepository>();
            _mockProductItemRepository = new Mock<IProductItemRespository>();
            _mockUserCouponRepository = new Mock<IUserCouponRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepository.Object);
            _mockUnitOfWork.Setup(u => u.CartItems).Returns(_mockCartItemRepository.Object);
            _mockUnitOfWork.Setup(u => u.Shippings).Returns(_mockShippingRepository.Object);
            _mockUnitOfWork.Setup(u => u.ProductItem).Returns(_mockProductItemRepository.Object);
            _mockUnitOfWork.Setup(u => u.UserCoupon).Returns(_mockUserCouponRepository.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            }, "mock"));

            _mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(new DefaultHttpContext { User = user });

            _orderService = new OrderService(_mockUnitOfWork.Object, _mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order { Id = Guid.NewGuid() } };
            _mockOrderRepository.Setup(repo => repo.GetOrders(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(orders);
            _mockOrderRepository.Setup(repo => repo.GetTotalCount(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(orders.Count);

            // Act
            var result = await _orderService.GetAll(1, 10);

            // Assert
            var successResult = Assert.IsType<SuccessDataResult<object>>(result);
            var response = successResult.Result as dynamic;
            //Assert.Equal(orders, response.Data);
        }

        [Fact]
        public async Task AddFromCart_WhenCalled_ReturnsSuccessResult()
        {
            // Arrange
            var cartId = new Guid();
            var cartItems = new List<CartItem>
            {
                new CartItem { Id= cartId, ProductItemId = Guid.NewGuid(), Count = 1 }
            };
            var userId = _mockHttpContextAccessor.Object.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shipping = new Shipping { Id = Guid.NewGuid() };
            var orderItems = new List<OrderItem>
            {
                new OrderItem { ProductItemId = Guid.NewGuid(), Quantity = 1, Price = 100, Subtotal = 100 }
            };
            var order = new Order { Id = Guid.NewGuid(), UserId = new Guid(userId) };

            _mockCartItemRepository.Setup(repo => repo.GetListCartItems(It.IsAny<List<Guid>>()))
                .Returns(cartItems);
            _mockProductItemRepository.Setup(repo => repo.GetById(It.IsAny<Guid>()))
                .Returns(new ProductVariant { Id = Guid.NewGuid(), Price = 100 });
            _mockShippingRepository.Setup(repo => repo.AddAsync(It.IsAny<Shipping>()))
                .Returns(Task.CompletedTask);
            _mockOrderRepository.Setup(repo => repo.AddAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);
            _mockOrderRepository.Setup(repo => repo.SaveChangeAsync())
                .Returns(Task.CompletedTask);
            _mockOrderRepository.Setup(repo => repo.GetOrder(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.AddFromCart(new CreateNewOrderFromCartDto
            {
                CartItemIds = new List<Guid> { cartId },
                DeliveryId = Guid.NewGuid(),
                DeliveryDto = new CreateDeliveryDto(),
                            });

            // Assert
            var successResult = Assert.IsType<SuccessResult>(result);
            Assert.Equal("Add order successfully", successResult.Message);
        }

        [Fact]
        public async Task UpdateOrderStatus_WhenOrderExists_ReturnsUpdatedOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderStatus = OrderStatus.Completed;
            var order = new Order { Id = orderId, OrderStatus = OrderStatus.Pending };

            _mockOrderRepository.Setup(repo => repo.GetOrder(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(order);
            _mockOrderRepository.Setup(repo => repo.Update(It.IsAny<Order>()));

            // Act
            var result = await _orderService.UpdateOrderStatus(orderId, orderStatus);

            // Assert
            var successResult = Assert.IsType<SuccessDataResult<Order>>(result);
            Assert.Equal(orderStatus, (successResult.Result as Order).OrderStatus);
        }

        [Fact]
        public async Task GetById_WhenOrderExists_ReturnsOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId };

            _mockOrderRepository.Setup(repo => repo.GetOrder(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetById(orderId);

            // Assert
            var successResult = Assert.IsType<SuccessDataResult<Order>>(result);
            Assert.Equal(order, (successResult.Result as Order));
        }

        [Fact]
        public async Task GetOrdersByUserId_WhenOrdersExist_ReturnsOrders()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), UserId = userId }
            };

            _mockOrderRepository.Setup(repo => repo.GetOrders(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetOrdersByUserId(userId, 1, 10);

            // Assert
            var successResult = Assert.IsType<SuccessDataResult<List<Order>>>(result);
            Assert.Equal(orders, successResult.Result);
        }
    }
}
