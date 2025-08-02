namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Linq.Expressions;

    /// <summary>
    /// The BaseRepository class provides a foundational implementation for a generic repository pattern within the PizzaApp.Data.Repository namespace. 
    /// It encapsulates common data access operations for entities, leveraging Entity Framework Core for database interactions. 
    /// This abstract class is designed to be extended by specific repository implementations for different entity types.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity managed by this repository. It must be a class, implement IEntity<TKey>, and have a parameterless constructor.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for TEntity. It must be a non-nullable type.</typeparam>
    /// <typeparam name="TRepository">The concrete type of the repository inheriting from BaseRepository. This is used for fluent API chaining.</typeparam>
    public abstract class BaseRepository<TEntity, TKey, TRepository>: IRepository<TEntity, TKey, TRepository> 
        where TEntity : class, IEntity<TKey>, new()
        where TKey : notnull
        where TRepository : BaseRepository<TEntity, TKey, TRepository>
    {
        /// <summary>
        /// The Entity Framework Core database context instance used for database operations.
        /// </summary>
        protected readonly PizzaAppContext DbContext;

        /// <summary>
        /// The DbSet representing the collection of TEntity in the DbContext.
        /// </summary>
        protected readonly DbSet<TEntity> DbSet;

        /// <summary>
        /// A flag indicating whether global query filters should be ignored for the current query. This flag is reset after ApplyConfiguration is called.
        /// </summary>
        protected bool ShouldIgnoreFilters = false;

        /// <summary>
        /// A flag indicating whether the query should be executed without change tracking. This flag is reset after ApplyConfiguration is called.
        /// </summary>
        protected bool ShouldNotTrack = false;

        /// <summary>
        /// Initializes a new instance of the BaseRepository class.
        /// </summary>
        /// <param name="dbContext">The PizzaAppContext instance to be used by the repository.</param>
        public BaseRepository(PizzaAppContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        /// <summary>
        /// Configures the current repository instance to ignore global query filters for the next query operation. 
        /// This is useful for retrieving entities that might otherwise be filtered out (e.g., soft-deleted entities).
        /// </summary>
        /// <returns>The current TRepository instance, allowing for method chaining.</returns>
        public TRepository IgnoreFiltering()
        {
            ShouldIgnoreFilters = true;
            return (TRepository)this;
        }
        /// <summary>
        /// Configures the current repository instance to execute the next query operation without change tracking. 
        /// This can improve performance for read-only scenarios as Entity Framework Core will 
        /// not track changes to the retrieved entities.
        /// </summary>
        /// <returns>The current TRepository instance, allowing for method chaining.</returns>
        public TRepository DisableTracking()
        {
            ShouldNotTrack = true;
            return (TRepository)this;
        }

        /// <summary>
        /// Asynchronously adds a single entity to the DbSet. 
        /// The changes are not persisted to the database until SaveChangesAsync() is called.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        /// <returns>A Task that represents the asynchronous add operation.</returns>
        public async Task AddAsync(TEntity item)
        {
            await this.DbSet.AddAsync(item);
        }

        /// <summary>
        /// Asynchronously adds a range of entities to the DbSet. 
        /// The changes are not persisted to the database until SaveChangesAsync() is called.
        /// </summary>
        /// <param name="items">An enumerable collection of entities to add.</param>
        /// <returns></returns>
        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await this.DbSet.AddRangeAsync(items);
        }

        /// <summary>
        /// Marks an entity as soft-deleted if it implements the ISoftDeletable interface. 
        /// This method sets the IsDeleted property to true and then calls Update() to mark the entity as modified. 
        /// The changes are not persisted until SaveChangesAsync() is called.
        /// </summary>
        /// <param name="entity">The entity to soft-delete.</param>
        /// <returns>true if the entity was soft-deleted (i.e., it implements ISoftDeletable and was updated), otherwise false.</returns>
        public bool SoftDelete(TEntity entity)
        {
            if (entity is ISoftDeletable softDeletable)
            {
                softDeletable.IsDeleted = true;
                return this.Update(entity);
            }
            return false;
        }

        /// <summary>
        /// Asynchronously retrieves the first element of a sequence that satisfies a specified condition, 
        /// or a default value if no such element is found. 
        /// Applies any configured query options (e.g., AsNoTracking, IgnoreFilters).
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A Task that represents the asynchronous operation. 
        /// The task result contains the first element that satisfies the condition, or null if no such element is found.</returns>
        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();
            query = this.ApplyConfiguration(query);

            TEntity? result = await query.FirstOrDefaultAsync(predicate);
            this.DbContext.BypassToppingFilters = false;

            return result;
        }

        /// <summary>
        /// Applies the configured query options (AsNoTracking, IgnoreFilters) to the provided IQueryable. 
        /// After applying, the AsNoTracking and IgnoreFilters flags are reset to false
        /// </summary>
        /// <param name="query">The IQueryable to which configurations should be applied.</param>
        /// <returns>The configured IQueryable<TEntity>.</returns>
        protected IQueryable<TEntity> ApplyConfiguration(IQueryable<TEntity> query)
        {
            if (typeof(TEntity) == typeof(Pizza) && !this.ShouldIgnoreFilters)
            {
                this.DbContext.BypassToppingFilters = true;
            }
            if (this.ShouldNotTrack)
            {
                query = query.AsNoTracking();
                this.ShouldNotTrack = false;
            }
            if (this.ShouldIgnoreFilters)
            {
                query = query.IgnoreQueryFilters();
                this.ShouldIgnoreFilters = false;
            }

            return query;
        }

        /// <summary>
        /// Asynchronously retrieves all entities from the DbSet. Applies any configured query options (e.g., AsNoTracking, IgnoreFilters).
        /// </summary>
        /// <returns>A Task that represents the asynchronous operation. The task result contains an enumerable collection of all entities.</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();
            query = this.ApplyConfiguration(query);

            IEnumerable<TEntity> result = await query.ToListAsync();

            this.DbContext.BypassToppingFilters = false;

            return result;
        }

        public async Task<IEnumerable<TEntity>>TakeAsync(int take, int skip = 0)
        {
            IQueryable<TEntity> query = this.DbSet.Skip(skip).Take(take);
            query = this.ApplyConfiguration(query);

            IEnumerable<TEntity> result = await query.ToListAsync();
            this.DbContext.BypassToppingFilters = false;

            return result;
        }

        public async Task<int> TotalEntityCountAsync()
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();
            query = this.ApplyConfiguration(query);

            int result = await query.CountAsync();
            this.DbContext.BypassToppingFilters = false;

            return result;
        }

        /// <summary>
        /// Asynchronously retrieves an entity by its primary key. Applies any configured query options (e.g., AsNoTracking, IgnoreFilters).
        /// </summary>
        /// <param name="id">The primary key of the entity to retrieve.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the entity with the specified ID, or null if not found.</returns>
        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();
            query = this.ApplyConfiguration(query);
            TEntity? result = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
            this.DbContext.BypassToppingFilters = false;

            return result;
        }

        /// <summary>
        /// Marks an entity for hard deletion from the database. This method removes the entity from the DbSet. The changes are not persisted until 
        /// </summary>
        /// <param name="entity">The entity to hard-delete.</param>
        /// <returns>true if the entity's state was marked as Deleted, otherwise false.</returns>
        public bool HardDelete(TEntity entity)
        {
            EntityEntry<TEntity> changeTrackerEntry = this.DbSet.Remove(entity);
            return changeTrackerEntry.State == EntityState.Deleted;
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>A Task that represents the asynchronous save operation.</returns>
        public async Task SaveChangesAsync()
        {
            await this.DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously retrieves the single element of a sequence that satisfies a specified condition, or a default value if no such element is found. 
        /// Throws an exception if more than one element satisfies the condition. Applies any configured query options (e.g., AsNoTracking, IgnoreFilters).
        /// </summary>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <returns>A Task that represents the asynchronous operation. 
        /// The task result contains the single element that satisfies the condition, or null if no such element is found.</returns>
        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();
            query = this.ApplyConfiguration(query);

            TEntity? result = await query.SingleOrDefaultAsync(predicate);
            this.DbContext.BypassToppingFilters = false;

            return result;
        }

        /// <summary>
        /// Marks an entity as modified in the DbContext. 
        /// The changes are not persisted to the database until SaveChangesAsync() is called.
        /// </summary>
        /// <param name="item">The entity to update.</param>
        /// <returns>true if the entity's state was marked as Modified, otherwise false.</returns>
        public bool Update(TEntity item)
        {
            EntityEntry changeTrackerEntry = this.DbSet.Update(item);
            return changeTrackerEntry.State == EntityState.Modified;
        }
        
        /// <summary>
        /// Asynchronously checks if any entity in the DbSet satisfies a specified condition. 
        /// This method always performs a no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A Task that represents the asynchronous operation. 
        /// The task result contains true if any entity satisfies the condition, otherwise false.</returns>
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();
            query = this.ApplyConfiguration(query);

            bool result = await query.AsNoTracking().AnyAsync(predicate);
            this.DbContext.BypassToppingFilters = false;

            return result;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of entities based on a list of their primary keys. 
        /// Applies any configured query options (e.g., AsNoTracking, IgnoreFilters). 
        /// Throws an InvalidOperationException if not all entities for the provided IDs are found.
        /// </summary>
        /// <param name="ids">An enumerable collection of primary keys.</param>
        /// <returns>A Task that represents the asynchronous operation. 
        /// The task result contains an enumerable collection of entities corresponding to the provided IDs.</returns>
        /// <exception cref="InvalidOperationException">InvalidOperationException if the count of retrieved entities does not match the count of provided IDs.</exception>
        public async Task<IEnumerable<TEntity>> GetRangeByIdsAsync(IEnumerable<TKey> ids)
        {
            IQueryable<TEntity> query = this.DbSet.AsQueryable();
            query = this.ApplyConfiguration(query);
            IEnumerable<TEntity> result = await query.Where(e => ids.Contains(e.Id)).ToListAsync();
            if (result.Count() != ids.Count())
            {
                throw new InvalidOperationException("Not all entities were found for the provided IDs.");
            }
            this.DbContext.BypassToppingFilters = false;

            return result;
        }
    }
}

