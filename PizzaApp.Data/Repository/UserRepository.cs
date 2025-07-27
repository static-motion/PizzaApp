namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Threading.Tasks;

    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        public UserRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }

        public Task<User?> GetUserWithAddressesAndCartAsync(Guid userId)
        {
            IQueryable<User> query = this.DbContext.Users.Where(u => u.Id == userId);
            query = IncludeShoppingCart(query)
                .Include(u => u.Addresses);

            return query.FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserWithAddressesAsync(Guid userId)
        {
            return await this.DbContext.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync();
        }

        public Task<User?> GetUserWithShoppingCartAsync(Guid userId)
        {
            IQueryable<User> query = this.DbContext.Users
                .Where(u => u.Id == userId);

            query = IncludeShoppingCart(query);

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
