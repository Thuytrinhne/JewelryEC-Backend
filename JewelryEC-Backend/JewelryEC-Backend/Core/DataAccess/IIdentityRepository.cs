using JewelryEC_Backend.Core.Entity;
using System.Linq.Expressions;
using System.Security.Principal;

namespace JewelryEC_Backend.Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task AddAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task MultiAddAsync(T[] entities);
    }
}
