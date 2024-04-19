using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICatalogRepository Catalogs { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IShippingRepository Shippings { get; }
        IProductCouponRepository ProductCoupons { get; }
        int Save();
    }
}
