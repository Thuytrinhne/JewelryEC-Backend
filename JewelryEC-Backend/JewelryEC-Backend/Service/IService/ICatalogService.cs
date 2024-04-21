using JewelryEC_Backend.Models.Catalogs.Entities;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Service.IService
{
    public interface ICatalogService
    {
        bool CreateCatalog(Catalog catalogToCreate);
        IEnumerable<Catalog> ListCatalogs();
        Catalog GetCatalogById(Guid id);
        IEnumerable<Catalog> FilterCatalogs(Guid? parentId = null, string name = null);
        bool UpdateCatalog(Catalog catalogToUpdate);
        bool DeleteCatalog(Guid id);
    }
}
