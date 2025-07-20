namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class PizzaRepository : BaseRepository<Pizza, int>, IPizzaRepository
    {
        public PizzaRepository(PizzaAppContext dbContext) 
            : base(dbContext)
        {
        }

        public Task<Pizza?> GetByIdWithIngredientsAsync(int id)
        {
            return this.DbSet.Include(p => p.Dough)
                .Include(p => p.Sauce)
                .Include(p => p.Toppings)
                    .ThenInclude(pt => pt.Topping)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
