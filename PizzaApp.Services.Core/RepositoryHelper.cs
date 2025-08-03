namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Repository;
    using PizzaApp.Data.Repository.Interfaces;

    public static class RepositoryHelper
    {
        public static async Task<Dictionary<TKey, TEntity>> GetEntityLookup<TKey, TEntity, TRepository>(IRepository<TEntity, TKey, TRepository> repository, bool ignoreFiltering = false)
            where TKey : notnull 
            where TEntity : class, IEntity<TKey>, new()
            where TRepository : IRepository<TEntity, TKey, TRepository>
        {
            repository.DisableTracking();

            if (ignoreFiltering)
                repository.IgnoreFiltering();

            IEnumerable<TEntity> entities = await repository.GetAllAsync();
            if (entities == null)
            {
                return new Dictionary<TKey, TEntity>();
            }

            return entities.ToDictionary(entity => entity.Id, entity => entity);
        }
    }
}
