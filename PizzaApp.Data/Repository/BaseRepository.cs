namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Linq.Expressions;
    using System.Reflection;
    using PizzaApp.Data.Models.Interfaces;

    public abstract class BaseRepository<TEntity, TKey> 
        : IAsyncRepository<TEntity, TKey> where TEntity : class
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
            if (asNoTracking)
            {
                return await this.DbSet.AsNoTracking().ToArrayAsync();
            }
            return await this.DbSet.ToArrayAsync();
        }

        public IQueryable<TEntity> GetAllAttached()
        {
            return this.DbSet.AsQueryable();
        }

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
            TEntity? entity = await this.DbSet.SingleOrDefaultAsync(predicate);
            return entity;
        }

        public bool Update(TEntity item)
        {
            EntityEntry changeTrackerEntry = this.DbSet.Update(item);
            return changeTrackerEntry.State == EntityState.Modified;
        }

        protected void PerformSoftDeleteOfEntity(TEntity entity)
        {
            PropertyInfo? isDeletedProperty
                = this.GetIsDeletedProperty(entity)
                ?? throw new InvalidOperationException();

            isDeletedProperty.SetValue(entity, true);
        }

        protected PropertyInfo? GetIsDeletedProperty(TEntity entity)
        {
            PropertyInfo? propertyInfo = entity
                .GetType()
                .GetProperty("IsDeleted", typeof(bool));
            return propertyInfo;
        }
    }
}
}
