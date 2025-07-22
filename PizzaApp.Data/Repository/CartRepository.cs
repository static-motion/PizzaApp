namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Threading.Tasks;

    public class CartRepository : BaseRepository<ShoppingCart, int>, ICartRepository
    {
        public CartRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }

        public Task<ShoppingCart?> GetCompleteCartByUserIdAsync(Guid userId)
        {
            return this.DbContext.ShoppingCarts
                .Include(c => c.Pizzas)
                .Include(c => c.Drinks)
                .Include(c => c.Desserts)
                .SingleOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
