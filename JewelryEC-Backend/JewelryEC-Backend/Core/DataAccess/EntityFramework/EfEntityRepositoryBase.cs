using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected AppDbContext context;

        public EfEntityRepositoryBase(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await context.FindAsync<TEntity>(filter);
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? await context.Set<TEntity>().ToListAsync()
                : await context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
                return context.Set<TEntity>().SingleOrDefault(filter);
        }

        public void Add(TEntity entity)
        {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChangesAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await context.AddAsync(entity);
        }

        public async Task Delete(TEntity entity)
        {
            context.Remove(entity);
        }

        public async Task MultiAddAsync(TEntity[] entities)
        {
            await context.AddRangeAsync(entities);
        }
        public async Task Update(TEntity entity)
        {
            context.Update(entity);
        }
    }
}