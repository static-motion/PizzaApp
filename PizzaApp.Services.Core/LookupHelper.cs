namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Repository;
    using PizzaApp.Data.Repository.Interfaces;

    public static class LookupHelper
    {
        public static async Task<Dictionary<TKey, TEntity>> GetEntityLookup<TKey, TEntity, TRepository>(IRepository<TEntity, TKey, TRepository> repository)
            where TKey : notnull 
            where TEntity : class, IEntity<TKey>, new()
            where TRepository : BaseRepository<TEntity, TKey, TRepository>
        {
            IEnumerable<TEntity> entities = await repository.DisableTracking().GetAllAsync();
            if (entities == null)
            {
                return new Dictionary<TKey, TEntity>();
            }

            return entities.ToDictionary(entity => entity.Id, entity => entity);
        }
    }
}
