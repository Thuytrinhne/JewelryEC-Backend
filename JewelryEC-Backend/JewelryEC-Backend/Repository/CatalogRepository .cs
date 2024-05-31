using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JewelryEC_Backend.Repository
{
    public class CatalogRepository : GenericRepository<Catalog>, ICatalogRepository
    {
        public CatalogRepository(AppDbContext context) : base(context)
        {
        }

        public async  Task<PaginationResult<Catalog>> GetCatalogsByPage(PaginationRequest request, Guid? parentId = null, string name = null)
        {
            // get order with pagination
            // return result
           
            var totalCount = await _context.Catalogs.LongCountAsync();
            List<Catalog> catalogs;
                if     (parentId is not null && name is not null)
                {
                    catalogs = await _context.Catalogs
                                       .Where(o => o.ParentId == parentId && o.Name == Name)
                                       .OrderBy(o => o.Name)
                                       .Skip(request.PageSize * request.PageIndex)
                                       .Take(request.PageSize)
                                       .ToListAsync();
                }
                else if (parentId is not null)
                {
                 catalogs = await _context.Catalogs
                                    .Where(o => o.ParentId == parentId)
                                    .OrderBy(o => o.Name)
                                    .Skip(request.PageSize * request.PageIndex)
                                    .Take(request.PageSize)
                                    .ToListAsync();
                }
                else if (name is not null)
                 {
                catalogs = await _context.Catalogs
                                   .Where(o => o.Name == name)
                                   .OrderBy(o => o.Name)
                                   .Skip(request.PageSize * request.PageIndex)
                                   .Take(request.PageSize)
                                   .ToListAsync();
                 }
                    else
                {
                    catalogs = await _context.Catalogs
                                       .OrderBy(o => o.Name)
                                       .Skip(request.PageSize * request.PageIndex)
                                       .Take(request.PageSize)
                                       .ToListAsync();
                }    


            return new PaginationResult<Catalog>(
                    request.PageIndex,
                    request.PageSize,
                    totalCount,
                    catalogs);
        }

       
    }
}
