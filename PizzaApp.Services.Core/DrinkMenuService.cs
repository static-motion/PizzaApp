namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Menu;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static PizzaApp.Services.Common.ExceptionMessages;

    public class DrinkMenuService : IDrinkMenuService
    {
        private readonly IDrinkRepository _drinkRepository;

        public DrinkMenuService(IDrinkRepository drinkRepository)
        {
            this._drinkRepository = drinkRepository;
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllBaseItemsAsync()
        {
            IEnumerable<Drink> allDrinks = await this._drinkRepository
                .DisableTracking()
                .GetAllAsync();

            return allDrinks
                .Select(d => new MenuItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl,
                    Category = MenuCategory.Drinks
                });
        }

        public async Task<MenuItemDetailsViewModel> GetDetailsById(int id)
        {
            Drink drink = await this._drinkRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(EntityNotFoundMessage, nameof(Drink), id);

            return new MenuItemDetailsViewModel()
            {
                Id = drink.Id,
                Name = drink.Name,
                Category = MenuCategory.Drinks,
                Description = drink.Description,
                ImageUrl = drink.ImageUrl,
                Price = drink.Price
            };
        }
    }
}
