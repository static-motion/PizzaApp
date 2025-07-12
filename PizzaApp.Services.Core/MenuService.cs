namespace PizzaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;

    public class MenuService : IMenuService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IDrinkRepository _drinkRepository;
        private readonly IDessertRepository _dessertRepository;

        public MenuService(IPizzaRepository pizzaRepository, IDrinkRepository drinkRepository, IDessertRepository dessertRepository)
        {
            this._pizzaRepository = pizzaRepository;
            this._drinkRepository = drinkRepository;
            this._dessertRepository = dessertRepository;
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllPizzasForMenuAsync()
        {
            return await this._pizzaRepository
                .GetAllAttached()
                .Select(p => new MenuItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl

                }).ToListAsync();
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllDrinksForMenuAsync()
        {
            return await this._drinkRepository
                .GetAllAttached()
                .Select(d => new MenuItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl

                }).ToListAsync();
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllDessertsForMenuAsync()
        {
            return await this._dessertRepository
                .GetAllAttached()
                .Select(d => new MenuItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl

                }).ToListAsync();
        }
    }
}
