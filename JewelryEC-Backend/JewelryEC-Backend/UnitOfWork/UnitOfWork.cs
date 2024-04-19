using JewelryEC_Backend.Data;
using JewelryEC_Backend.Repository;
using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICatalogRepository Catalogs { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IProductRepository Products { get; private set; }
        public IShippingRepository Shippings { get; private set; }
        public IProductCouponRepository ProductCoupons { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Catalogs = new CatalogRepository(_context);
            Orders = new OrderRepository(_context);
            Products = new ProductRepository(_context);
            Shippings = new ShippingRepository(_context);
            ProductCoupons = new ProductCouponRepository(_context);
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
