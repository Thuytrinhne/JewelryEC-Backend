using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Models.Catalogs.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Service.IService
{
    public interface ICatalogService
    {
        Guid CreateCatalog(Catalog catalogToCreate);
        IEnumerable<Catalog> ListCatalogs();
        Catalog GetCatalogById(Guid id);
        IEnumerable<Catalog> FilterCatalogs(Guid? parentId = null, string name = null);
        bool UpdateCatalog(Catalog catalogToUpdate);
        bool DeleteCatalog(Guid id);
        Task<PaginationResult<Catalog>> GetCatalogsByPage(PaginationRequest request, Guid? parentId, [FromQuery] string? name);
    }
}
