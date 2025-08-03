namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ToppingCategoryRepository : BaseRepository<ToppingCategory, int, IToppingCategoryRepository>, IToppingCategoryRepository
    {
        public ToppingCategoryRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<ToppingCategory>> GetAllWithToppingsAsync()
        {
            IQueryable<ToppingCategory> query = this.DbSet.Include(tc => tc.Toppings);

            query = this.ApplyConfiguration(query);

            return await query.ToArrayAsync();
        }
    }
}
