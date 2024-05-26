using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading;

namespace JewelryEC_Backend.Repository
{
    public class CatalogRepository : GenericRepository<Catalog>, ICatalogRepository
    {
        public CatalogRepository(AppDbContext context) : base(context)
        {
        }

        public async  Task<PaginationResult<Catalog>> GetCatalogsByPage(PaginationRequest request)
        {
            // get order with pagination
            // return result
           
            var totalCount = await _context.Catalogs.LongCountAsync();

            var catalogs = await _context.Catalogs
                                .OrderBy(o => o.Name)
                                .Skip(request.PageSize * request.PageIndex)
                                .Take(request.PageSize)
                                .ToListAsync();

            return new PaginationResult<Catalog>(
                    request.PageIndex,
                    request.PageSize,
                    totalCount,
                    catalogs);
        }

       
    }
}
