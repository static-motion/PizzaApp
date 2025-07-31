namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OrderRepository : BaseRepository<Order, Guid, OrderRepository>, IOrderRepository
    {
        public OrderRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            IQueryable<Order> query = this.DbSet.Where(o => o.UserId == userId)
                .Include(o => o.OrderPizzas)
                    .ThenInclude(op => op.Toppings)
                .Include(o => o.OrderPizzas)
                    .ThenInclude(p => p.BasePizza)
                .Include(o => o.OrderDeserts)
                    .ThenInclude(od => od.Dessert)
                .Include(o => o.OrderDrinks)
                    .ThenInclude(od => od.Drink);

            query = this.ApplyConfiguration(query);
            return await query.ToListAsync();
        }
    }
}
