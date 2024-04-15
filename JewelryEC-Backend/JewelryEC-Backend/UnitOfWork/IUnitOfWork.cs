using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICatalogRepository Catalogs { get; }
        int Save();
    }
}
