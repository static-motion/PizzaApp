namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Repository.Interfaces;

    public static class LookupHelper
    {
        public static async Task<Dictionary<TKey, IEntity>> GetEntityLookup<TKey, IEntity>(IRepository<IEntity, TKey> repository) 
            where TKey : notnull 
            where IEntity : class, IEntity<TKey>
        {
            IEnumerable<IEntity> entities = await repository.GetAllAsync(asNoTracking: true);
            if (entities == null)
            {
                return new Dictionary<TKey, IEntity>();
            }

            return entities.ToDictionary(entity => entity.Id, entity => entity);
        }
    }
}
