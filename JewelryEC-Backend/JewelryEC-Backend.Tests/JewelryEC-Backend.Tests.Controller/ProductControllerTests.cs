using JewelryEC_Backend.Models.Products.Dto;
using JewelryEC_Backend.Models.Products;
using System.Drawing.Printing;
using JewelryEC_Backend.Core.Utilities.Results;
using Moq;

namespace JewelryEC_Backend.Tests.JewelryEC_Backend.Tests.Controller;
public class ProductControllerTests
{
   private readonly Mock<IMapper> _mockMapper;
   private readonly Mock<IProductService> _mockService;
   private readonly ProductApiController _ProductController;
   private readonly ResponseDto _response;


    public ProductControllerTests()
    {
      _mockMapper = new Mock<IMapper>();
      _mockService = new Mock<IProductService>();
      _ProductController = new ProductApiController(_mockService.Object);
      _response = new ResponseDto();
    }
    [Fact]
    public async Task Get_WhenProductsExist_ReturnsOkResult()
    {
        // Arrange
        var parentId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
        var name = "Product 1";
        List<Product> Products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Product 1" },
        };
        _mockService.Setup(s => s.GetAll(1, 10)).ReturnsAsync(new SuccessDataResult<List<Product>>(Products));

        // Act
        var result = await _ProductController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var responseDto = okResult.Value as ResponseDto;
        Assert.NotNull(responseDto);
        Assert.Equal(Products, responseDto.Result);
    }
    //[Fact]
    //public async Task Add_WhenProductDtoIsValid_ReturnsOkResult()
    //{
    //    // Arrange
    //    var productDto = new CreateProductDto { Name = "New Product" };
    //    var product = new Product { Id = Guid.NewGuid(), Name = "New Product" };

    //    _mockService.Setup(s => s.Add(productDto)).ReturnsAsync(new SuccessDataResult<Guid>(product.Id));

    //    // Act
    //    var result = await _ProductController.Add(productDto);

    //    // Assert
    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    Assert.Equal(200, okResult.StatusCode);

    //    var responseDto = okResult.Value as ResponseDto;
    //    Assert.NotNull(responseDto);
    //    Assert.Equal(product.Id, responseDto.Result);
    //}

}


