namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Threading.Tasks;

    public class UserRepository : BaseRepository<User, Guid, IUserRepository>, IUserRepository
    {
        public UserRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }

        public Task<User?> GetUserWithAddressesAndCartAsync(Guid userId)
        {
            IQueryable<User> query = this.DbContext.Users.Where(u => u.Id == userId);
            query = IncludeShoppingCart(query)
                .Include(u => u.Addresses);

            query = this.ApplyConfiguration(query);

            return query.FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserWithAddressesAsync(Guid userId)
        {
            IQueryable<User> query = this.DbContext.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Addresses);

            query = this.ApplyConfiguration(query);
            return await query.FirstOrDefaultAsync();
        }

        public Task<User?> GetUserWithShoppingCartAsync(Guid userId)
        {
            IQueryable<User> query = this.DbContext.Users
                .Where(u => u.Id == userId);

            query = IncludeShoppingCart(query);
            query = this.ApplyConfiguration(query);

            return query.FirstOrDefaultAsync();
        }

        private static IQueryable<User> IncludeShoppingCart(IQueryable<User> query)
        {
            return query
                .Include(c => c.ShoppingCartPizzas)
                    .ThenInclude(scp => scp.BasePizza)
                .Include(c => c.ShoppingCartDrinks)
                    .ThenInclude(scd => scd.Drink)
                .Include(c => c.ShoppingCartDesserts)
                    .ThenInclude(scd => scd.Dessert);
        }
    }
}
