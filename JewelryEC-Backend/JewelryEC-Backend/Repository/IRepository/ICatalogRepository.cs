using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface ICatalogRepository : IGenericRepository<Catalog>
    {
        Task<PaginationResult<Catalog>> GetCatalogsByPage(PaginationRequest request, Guid? parentId = null, string name = null);

    }
}
