using JewelryEC_Backend.Models.Catalogs.Entities;


namespace JewelryEC_Backend.Tests.JewelryEC_Backend.Tests.Service;
public class CatalogServiceTests
{
    private readonly Mock<IUnitOfWork> _mockIUnitOfWork;
    private readonly CatalogService _catalogService;
    public CatalogServiceTests()
    {
        _mockIUnitOfWork = new Mock<IUnitOfWork>();
        _catalogService = new CatalogService(_mockIUnitOfWork.Object);
    }
    #region Test case for  ListCatalogs
    [Fact]
    public void ListCatalogs_WhenCatalogsExist_ReturnAllCatalogs()
    {
        // Arrange
        var expectedCatalogs = new List<Catalog>
        {
            new Catalog { Id = Guid.NewGuid(), Name = "Catalog 1", ParentId= new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200") }
        };
        _mockIUnitOfWork.Setup(s=>s.Catalogs.GetAll()).Returns(expectedCatalogs);

        // Act
        var result = _catalogService.ListCatalogs();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCatalogs, result);

    }

    [Fact]
    public void ListCatalogs_WhenNoCatalogsExist_ReturnEmptyList()
    {
        // Arrange
        var expectedCatalogs = new List<Catalog>();
        _mockIUnitOfWork.Setup(s => s.Catalogs.GetAll()).Returns(expectedCatalogs);

        // Act
        var result = _catalogService.ListCatalogs();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ListCatalogs_WhenGetAllThrowsException_ShouldThrowException()
    {
        // Arrange
        _mockIUnitOfWork.Setup(s => s.Catalogs.GetAll()).Throws(new System.Exception("Database error"));

        // Act & Assert
        Assert.Throws<System.Exception>(() => _catalogService.ListCatalogs());
    }
    #endregion
}
