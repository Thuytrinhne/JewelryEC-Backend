
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
    #region Test case for ListCatalogs
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
    #region Test case for CreateCatalog
    [Fact]
    public void CreateCatalog_WhenCatalogIsCreatedSuccessfully_ReturnExactIdCatalog()
    {

        // Arrange
        var catalog = new Catalog { Id = Guid.NewGuid(), Name = "Test Catalog" };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.Add(It.IsAny<Catalog>()));
        _mockIUnitOfWork.Setup(uow => uow.Save()).Verifiable();

        // Act
        var result = _catalogService.CreateCatalog(catalog);

        // Assert
        Assert.Equal(catalog.Id, result);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.Add(It.Is<Catalog>(c => c == catalog)), Times.Once);
        _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Once);
    }
    [Fact]
    public void CreateCatalog_WhenExceptionIsThrown_ShouldThrowException()
    {
        // Arrange
        var catalog = new Catalog { Id = Guid.NewGuid(), Name = "Test Catalog" };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.Add(It.IsAny<Catalog>())).Throws(new Exception("Database error"));

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _catalogService.CreateCatalog(catalog));
        Assert.Equal($"Could not create catalog {catalog.Id}", exception.Message);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.Add(It.Is<Catalog>(c => c == catalog)), Times.Once);
        _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Never); // Save should not be called due to the exception the exception
    }
    #endregion
    #region Test case for DeleteCatalog
    [Fact]
    public void DeleteCatalog_WhenExistingCatalog_ReturnTrue()
    {

         // Arrange
        Guid catalogId = Guid.NewGuid();
        Catalog existingCatalog = new Catalog { Id = catalogId, Name = "Test Catalog" };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.GetById(catalogId)).Returns(existingCatalog);
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.Remove(It.IsAny<Catalog>()));

        // Act
        bool result = _catalogService.DeleteCatalog(catalogId);

        // Assert
        Assert.True(result);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.Remove(It.IsAny<Catalog>()), Times.Once);
        _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Once);
    }


        [Fact]
    public void DeleteCatalog_WhenCatalogNotFound_ReturnFalse()
    {
        // Arrange
        var catalogId = Guid.NewGuid();
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.GetById(It.IsAny<Guid>())).Returns((Catalog)null);

        // Act
        var result = _catalogService.DeleteCatalog(catalogId);

        // Assert
        Assert.False(result);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.GetById(It.Is<Guid>(id => id == catalogId)), Times.Once);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.Remove(It.IsAny<Catalog>()), Times.Never);
        _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Never);
    }

    [Fact]
    public void DeleteCatalog_WhenExceptionIsThrown_ShouldThrowException()
    {
        // Arrange
        var catalogId = Guid.NewGuid();
        var catalog = new Catalog { Id = catalogId, Name = "Test Catalog" };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.GetById(It.IsAny<Guid>())).Returns(catalog);
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.Remove(It.IsAny<Catalog>())).Throws(new Exception("Database error"));

       

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _catalogService.DeleteCatalog(catalogId));
        Assert.Equal($"Could not delete catalog {catalogId}", exception.Message);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.GetById(It.Is<Guid>(id => id == catalogId)), Times.Once);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.Remove(It.Is<Catalog>(c => c == catalog)), Times.Once);
        _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Never);
    }

    #endregion
    #region Test case for FilterCatalogs
    [Fact]
    public void FilterCatalogs_WhenParentIdAndNameSpecified_ReturnFilteredCatalogs()
    {
        // Arrange
        Guid parentId = Guid.NewGuid();
        string name = "TestCatalog";
        IEnumerable<Catalog> expectedCatalogs = new List<Catalog>
        {
            new Catalog { Id = Guid.NewGuid(), ParentId = parentId, Name = name }

        };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.Find(It.IsAny<Expression<Func<Catalog, bool>>>()))
                       .Returns((Expression<Func<Catalog, bool>> predicate) => expectedCatalogs.Where(predicate.Compile()));

        // Act
        IEnumerable<Catalog> result = _catalogService.FilterCatalogs(parentId, name);

        // Assert
        Assert.Equal(expectedCatalogs, result);
    }
    [Fact]
    public void FilterCatalogs_WhenOnlyParentIdSpecified_ReturnFilteredCatalogs()
    {
        // Arrange
        Guid parentId = Guid.NewGuid();
        IEnumerable<Catalog> expectedCatalogs = new List<Catalog>
        {
            new Catalog { Id = Guid.NewGuid(), ParentId = parentId }
        };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.Find(It.IsAny<Expression<Func<Catalog, bool>>>()))
                       .Returns((Expression<Func<Catalog, bool>> predicate) => expectedCatalogs.Where(predicate.Compile()));

        // Act
        IEnumerable<Catalog> result = _catalogService.FilterCatalogs(parentId);

        // Assert
        Assert.Equal(expectedCatalogs, result);
    }
    [Fact]
    public void FilterCatalogs_WhenOnlyNameSpecified_ReturnFilteredCatalogs()
    {
        // Arrange
        string name = "TestCatalog";
        IEnumerable<Catalog> expectedCatalogs = new List<Catalog>
        {
            new Catalog { Id = Guid.NewGuid(), Name = name }
        };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.Find(It.IsAny<Expression<Func<Catalog, bool>>>()))
                       .Returns((Expression<Func<Catalog, bool>> predicate) => expectedCatalogs.Where(predicate.Compile()));

        // Act
        IEnumerable<Catalog> result = _catalogService.FilterCatalogs(name: name);

        // Assert
        Assert.Equal(expectedCatalogs, result);
    }

    [Fact]
    public void FilterCatalogs_WhenNeitherParentIdNorNameSpecified_ReturnAllCatalogs()
    {
        // Arrange
        IEnumerable<Catalog> expectedCatalogs = new List<Catalog>
        {
            new Catalog { Id = Guid.NewGuid(), Name = "Catalog1" },
            new Catalog { Id = Guid.NewGuid(), Name = "Catalog2" },
            new Catalog { Id = Guid.NewGuid(), Name = "Catalog3" }
        };
        _mockIUnitOfWork.Setup(s=>s.Catalogs.GetAll()).Returns(expectedCatalogs);

        // Act
        IEnumerable<Catalog> result = _catalogService.FilterCatalogs();

        // Assert
        Assert.Equal(expectedCatalogs, result);
    }

    #endregion
    #region Test case for GetCatalogById
    [Fact]
    public void GetCatalogById_WhenCatalogExists_ReturnCatalog()
    {
        // Arrange
        Guid catalogId = Guid.NewGuid();
        Catalog expectedCatalog = new Catalog { Id = catalogId, Name = "Test Catalog" };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.GetById(catalogId)).Returns(expectedCatalog);

        // Act
        Catalog result = _catalogService.GetCatalogById(catalogId);

        // Assert
        Assert.Equal(expectedCatalog, result);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.GetById(It.Is<Guid>(id => id == catalogId)), Times.Once);

    }

    [Fact]
    public void GetCatalogById_WhenCatalogDoesNotExist_ReturnNull()
    {
        // Arrange
        Guid catalogId = Guid.NewGuid();
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.GetById(catalogId)).Returns((Catalog)null);

        // Act
        Catalog result = _catalogService.GetCatalogById(catalogId);

        // Assert
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.GetById(It.Is<Guid>(id => id == catalogId)), Times.Once);
        Assert.Null(result);
    }
    [Fact]
    public void GetCatalogById_WhenExceptionThrown_ShouldThrowException()
    {
        // Arrange
        Guid catalogId = Guid.NewGuid();
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.GetById(It.IsAny<Guid>())).Throws(new Exception("Database error"));

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _catalogService.GetCatalogById(catalogId));
        Assert.Equal($"Could not found catalog {catalogId}", exception.Message);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.GetById(It.Is<Guid>(id => id == catalogId)), Times.Once);
    }
    #endregion
    #region Test case for UpdateCatalog
    [Fact]
    public void UpdateCatalog_WhenCatalogIsUpdatedSuccessfully_ReturnsTrue()
    {
        // Arrange
        Catalog catalogToUpdate = new Catalog { Id = Guid.NewGuid(), Name = "Updated Catalog" };
        Catalog existingCatalog = new Catalog { Id = catalogToUpdate.Id, Name = "Existing Catalog" };
        _mockIUnitOfWork.Setup(s => s.Catalogs.GetById(catalogToUpdate.Id)).Returns(existingCatalog);
        _mockIUnitOfWork.Setup(s => s.Catalogs.Update(catalogToUpdate));
        _mockIUnitOfWork.Setup(uow => uow.Save());
        // Act
        bool result = _catalogService.UpdateCatalog(catalogToUpdate);

        // Assert
        Assert.True(result);
        _mockIUnitOfWork.Verify(repo => repo.Catalogs.Update(It.Is<Catalog>(c => c == catalogToUpdate)), Times.Once);
        _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Once);
    }
    [Fact]
    public void UpdateCatalog_WhenCatalogDoesNotExist_ReturnsFalse()
    {
        // Arrange
        Catalog catalogToUpdate = new Catalog { Id = Guid.NewGuid(), Name = "Updated Catalog" };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.GetById(catalogToUpdate.Id)).Returns((Catalog)null);

        // Act
        bool result = _catalogService.UpdateCatalog(catalogToUpdate);

        // Assert
        Assert.False(result);
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.Update(It.IsAny<Catalog>()), Times.Never);
        _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Never);
    }

    [Fact]
    public void UpdateCatalog_WhenCatalogToUpdateIsNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => _catalogService.UpdateCatalog(null));
        Assert.Equal("The catalog to update cannot be null", ex.ParamName);

    }
    [Fact]
    public void UpdateCatalog_WhenExceptionIsThrown_ShouldThrowException()
    {
        // Arrange
        Catalog catalogToUpdate = new Catalog { Id = Guid.NewGuid(), Name = "Updated Catalog" };
        Catalog existingCatalog = new Catalog { Id = catalogToUpdate.Id, Name = "Existing Catalog" };
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.GetById(catalogToUpdate.Id)).Returns(existingCatalog);
        _mockIUnitOfWork.Setup(uow => uow.Catalogs.Update(catalogToUpdate)).Throws(new Exception("Update failed"));

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _catalogService.UpdateCatalog(catalogToUpdate));
        Assert.Equal(ex.Message, $"Could not update catalog {catalogToUpdate.Id}");
        _mockIUnitOfWork.Verify(uow => uow.Catalogs.Update(It.Is<Catalog>(c => c == catalogToUpdate)), Times.Once);
        _mockIUnitOfWork.Verify(uow => uow.Save(), Times.Never);
    }
    #endregion
}
