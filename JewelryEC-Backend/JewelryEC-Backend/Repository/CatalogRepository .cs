using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.Repository
{
    public class CatalogRepository : GenericRepository<Catalog>, ICatalogRepository
    {
        public CatalogRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Catalog> GetPopularCatalogs(int count)
        {
            throw new NotImplementedException();
        }
    }
}
