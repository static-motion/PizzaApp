namespace PizzaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using PizzaApp.Data;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PizzaService : IPizzaService
    {
        private readonly PizzaAppContext _dbContext;
        public PizzaService(PizzaAppContext context)
        {
            this._dbContext = context;
        }

        public async Task<IEnumerable<MenuPizzaViewModel>> GetAllPizzasForMenuAsync()
        {
            return await this._dbContext.Pizzas
                .Select(p => new MenuPizzaViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl

                }).ToListAsync();
        }
    }
}
