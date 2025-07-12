namespace PizzaApp.Data.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity, TKey>
    {
        Task AddAsync(TEntity item);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

        IQueryable<TEntity> GetAllAttached();

        ValueTask<TEntity?> GetByIdAsync(TKey id);

        bool HardDelete(TEntity entity);

        Task SaveChangesAsync();

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        bool SoftDelete(TEntity entity);

        bool Update(TEntity item);
    }
}
