using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Tests.JewelryEC_Backend.Tests.Controller;
public class CatalogControllerTests
{
   private readonly Mock<IMapper> _mockMapper;
   private readonly Mock<ICatalogService> _mockService;
   private readonly CatalogAPIController _catalogController;
   private readonly ResponseDto _response;


    public CatalogControllerTests()
    {
      _mockMapper = new Mock<IMapper>();
      _mockService = new Mock<ICatalogService>();
      _catalogController = new CatalogAPIController(_mockMapper.Object, _mockService.Object);
      _response = new ResponseDto();
    }
    #region Test case for  Get ALL 
    [Fact]
    public async Task Get_WhenCatalogsExist_ReturnsOkResult()
    {
        // Arrange
        var parentId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
        var name = "Catalog 1";
        var catalogs = new List<Catalog>
        {
            new Catalog { Id = Guid.NewGuid(), Name = "Catalog 1", ParentId= new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200") },
        };
        var catalogDtos = new List<GetCatalogResponseDto>
        {
            new GetCatalogResponseDto { Id = catalogs[0].Id, Name = "Catalog 1", ParentId= new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200") }
        };

        _mockService.Setup(s => s.FilterCatalogs(parentId, name)).Returns(catalogs);
        _mockMapper.Setup(m => m.Map<IEnumerable<GetCatalogResponseDto>>(catalogs)).Returns(catalogDtos);

        // Act
        var result = await _catalogController.Get(parentId, name);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var responseDto = okResult.Value as ResponseDto;
        Assert.NotNull(responseDto);
        Assert.Equal(catalogDtos, responseDto.Result);
    }


    [Fact]
    public async Task Get_WhenNoCatalogsExist_ReturnNotFound()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var name = "TestCatalog";
        _mockService.Setup(s => s.FilterCatalogs(parentId, name)).Returns(new List<Catalog>());

        // Act
        var result = await _catalogController.Get(parentId, name);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("Parent Id is not valid or no catalog with this parentId", notFoundResult.Value);
    }

    [Fact]
    public async Task Get_WhenExceptionOccurs_ReturnStatusCode500()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var name = "TestCatalog";
        _mockService.Setup(s => s.FilterCatalogs(parentId, name)).Throws(new Exception("Database error"));

        // Act
        var result = await _catalogController.Get(parentId, name);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        var responseDto = Assert.IsType<ResponseDto>(statusCodeResult.Value);
        Assert.False(responseDto.IsSuccess);
        Assert.Contains("Database error", responseDto.Message);
    }
    #endregion
}


