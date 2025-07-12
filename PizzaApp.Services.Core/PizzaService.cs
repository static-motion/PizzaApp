namespace PizzaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;

    public class PizzaService : IPizzaService
    {
        private readonly IPizzaRepository _pizzaRepository;
        public PizzaService(IPizzaRepository repository)
        {
            this._pizzaRepository = repository;
        }

        public async Task<IEnumerable<MenuPizzaViewModel>> GetAllPizzasForMenuAsync()
        {
            return await this._pizzaRepository
                .GetAllAttached()
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
