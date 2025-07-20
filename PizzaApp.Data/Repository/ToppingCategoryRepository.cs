namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ToppingCategoryRepository : BaseRepository<ToppingCategory, int>, IToppingCategoryRepository
    {
        public ToppingCategoryRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Topping>> GetAllToppingsFromRangeAsync(IEnumerable<int> selectedToppingIds)
        {
            return await this.DbSet
                .SelectMany(t => t.Toppings)
                .Where(tc => selectedToppingIds.Contains(tc.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<ToppingCategory>> GetAllWithToppingsAsync(bool asNoTracking = false)
        {
            var query = this.DbSet.Include(tc => tc.Toppings);

            if (asNoTracking)
            {
                query.AsNoTracking();
            }

            return await query.ToArrayAsync();
        }
    }
}
