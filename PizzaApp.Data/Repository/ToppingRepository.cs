namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ToppingRepository : BaseRepository<Topping, int, ToppingRepository>, IToppingRepository
    {
        public ToppingRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Topping>> GetAllToppingsFromRangeAsync(IEnumerable<int> selectedToppingIds)
        {
            IQueryable<Topping> query = this.DbSet
                .Where(tc => selectedToppingIds.Contains(tc.Id));

            query = this.ApplyConfiguration(query);
            return await query.ToListAsync();
        }
    }
}
