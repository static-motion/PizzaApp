namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity, TKey, TRepository> 
        where TEntity : class, IEntity<TKey>, new()
        where TKey : notnull
        where TRepository : BaseRepository<TEntity, TKey, TRepository>
    {
        Task AddAsync(TEntity item);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetRangeByIdsAsync(IEnumerable<TKey> ids);

        Task<TEntity?> GetByIdAsync(TKey id);

        bool HardDelete(TEntity entity);

        Task SaveChangesAsync();

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        bool SoftDelete(TEntity entity);

        bool Update(TEntity item);

        //IQueryable<TEntity> ApplyConfiguration(IQueryable<TEntity> query);

        TRepository DisableTracking();

        TRepository IgnoreFiltering();
    }
}
