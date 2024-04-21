using JewelryEC_Backend.Repository.IRepository;

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

        int Save();
    }
}
