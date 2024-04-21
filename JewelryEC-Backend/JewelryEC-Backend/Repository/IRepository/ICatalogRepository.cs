using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface ICatalogRepository : IGenericRepository<Catalog>
    {
        IEnumerable<Catalog> GetPopularCatalogs(int count);
    }
}
