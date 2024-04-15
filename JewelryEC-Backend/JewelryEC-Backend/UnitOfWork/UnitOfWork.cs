using JewelryEC_Backend.Data;
using JewelryEC_Backend.Repository;
using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICatalogRepository Catalogs { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Catalogs = new CatalogRepository(_context);
        }
     

        public int Save()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
