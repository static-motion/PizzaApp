namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Collections.Generic;
    using GCommon.Enums;

    public class PizzaRepository : BaseRepository<Pizza, int, PizzaRepository>, IPizzaRepository
    {
        public PizzaRepository(PizzaAppContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<ICollection<Pizza>> GetAllBasePizzasAsync()
        {
            IQueryable<Pizza> query = this.DbSet.Where(p => p.PizzaType == PizzaType.BasePizza);
            query = this.ApplyConfiguration(query);

            return await query.ToListAsync();
        }

        public async Task<ICollection<Pizza>> TakeBasePizzasWithIngredientsAsync(int take, int skip = 0)
        {
            IQueryable<Pizza> query = this.DbSet
                .Where(p => p.PizzaType == PizzaType.BasePizza)
                .Include(p => p.Dough)
                .Include(p => p.Sauce)
                .Include(p => p.Toppings)
                    .ThenInclude(pt => pt.Topping)
                    .ThenInclude(t => t.ToppingCategory)
                .Skip(skip)
                .Take(take); 

            query = this.ApplyConfiguration(query);

            return await query.ToListAsync();
        }

        public async Task<Pizza?> GetByIdWithIngredientsAsync(int id)
        {
            IQueryable<Pizza> query = this.DbSet.Include(p => p.Dough)
                .Include(p => p.Sauce)
                .Include(p => p.Toppings)
                    .ThenInclude(pt => pt.Topping);

            query = this.ApplyConfiguration(query);

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Pizza> GetAllBasePizzasAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
