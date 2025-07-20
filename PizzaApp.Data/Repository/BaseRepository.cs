namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Linq.Expressions;

    public abstract class BaseRepository<TEntity, TKey> 
        : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly PizzaAppContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(PizzaAppContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity item)
        {
            await this.DbSet.AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await this.DbSet.AddRangeAsync(items);
        }

        public bool SoftDelete(TEntity entity)
        {
            if (entity is ISoftDeletable softDeletable)
            {
                softDeletable.IsDeleted = true;
                return this.Update(entity);
            }
            return false;
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
        {
            var query = this.DbSet.AsQueryable();
            if (asNoTracking)
            {
                query.AsNoTracking();
            }
            return await this.DbSet.ToArrayAsync();
        }

        /*public IQueryable<TEntity> GetAllAttached()
        {
            return this.DbSet.AsQueryable();
        }*/

        public ValueTask<TEntity?> GetByIdAsync(TKey id)
        {
            return this.DbSet.FindAsync(id);
        }

        public bool HardDelete(TEntity entity)
        {
            EntityEntry<TEntity> changeTrackerEntry = this.DbSet.Remove(entity);
            return changeTrackerEntry.State == EntityState.Deleted;
        }

        public async Task SaveChangesAsync()
        {
            await this.DbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.DbSet.SingleOrDefaultAsync(predicate);
        }

        public bool Update(TEntity item)
        {
            EntityEntry changeTrackerEntry = this.DbSet.Update(item);
            return changeTrackerEntry.State == EntityState.Modified;
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.AnyAsync(predicate);
        }
    }
}

