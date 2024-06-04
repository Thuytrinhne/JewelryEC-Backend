using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Catalogs.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net.WebSockets;

namespace JewelryEC_Backend.Core.Repository.EntityFramework
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }
        public async Task <PaginationResult<TEntity>> GetAllPagination(PaginationRequest pagination)
        {
            var totalCount = await _context.Set<TEntity>().LongCountAsync();

            var entities =   await  _context.Set<TEntity>().Skip(pagination.PageSize * pagination.PageIndex)
                                       .Take(pagination.PageSize)
                                       .ToListAsync();

            return new PaginationResult<TEntity>(
                    pagination.PageIndex,
                    pagination.PageSize,
                    totalCount,
                    entities);
        }
        public async Task<PaginationResult<TEntity>> FindPagination(PaginationRequest pagination, Expression<Func<TEntity, bool>> expression)
        {
            var totalCount = await _context.Set<TEntity>().LongCountAsync();

            var entities = await _context.Set<TEntity>()
                                       .Where(expression)
                                       .Skip(pagination.PageSize * pagination.PageIndex)
                                       .Take(pagination.PageSize)
                                       .ToListAsync();

            return new PaginationResult<TEntity>(
                    pagination.PageIndex,
                    pagination.PageSize,
                    totalCount,
                    entities);
        }

        public TEntity GetById(Guid id)
        {
            return _context.Set<TEntity>().Find(id);
        }
        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }
        public async Task Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.FindAsync<TEntity>(filter);
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>().ToList()
                : _context.Set<TEntity>().Where(filter).ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().FirstOrDefault(filter);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
            _context.SaveChanges();
        }

        public async Task Delete(TEntity entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
        public async Task MultiDelete(List<TEntity> entities)
        {
            _context.RemoveRange(entities);
        }
        public async Task MultiAddAsync(TEntity[] entities)
        {
            await _context.AddRangeAsync(entities);
        }
        public async Task SaveChangeAsync()
        {
            //_context.SaveChangesAsync();
        }

        public void Detach(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
