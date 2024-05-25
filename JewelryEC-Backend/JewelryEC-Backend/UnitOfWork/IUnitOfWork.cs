using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Service.IService;

namespace JewelryEC_Backend.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICatalogRepository Catalogs { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }
        IEmailVerificationRespository EmailVerifications { get; }
        IUserRespository Users { get; }
        IRoleRespository Roles { get; }

        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IShippingRepository Shippings { get; }
        IProductCouponRepository ProductCoupons { get; }
        IProductItemRespository ProductItem { get; }
        IUserCouponRepository UserCoupon { get; }

        int Save();
    }
}
