namespace PizzaApp.Data.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<IEntity, TKey>
    {
        Task AddAsync(IEntity item);

        Task AddRangeAsync(IEnumerable<IEntity> items);

        Task<IEntity?> FirstOrDefaultAsync(Expression<Func<IEntity, bool>> predicate);

        Task<IEnumerable<IEntity>> GetAllAsync(bool asNoTracking = false, bool ignoreQueryFilters = false);

        Task<IEnumerable<IEntity>> GetRangeByIdsAsync(IEnumerable<TKey> ids);

        ValueTask<IEntity?> GetByIdAsync(TKey id);

        bool HardDelete(IEntity entity);

        Task SaveChangesAsync();

        Task<IEntity?> SingleOrDefaultAsync(Expression<Func<IEntity, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<IEntity, bool>> predicate);

        bool SoftDelete(IEntity entity);

        bool Update(IEntity item);
    }
}
