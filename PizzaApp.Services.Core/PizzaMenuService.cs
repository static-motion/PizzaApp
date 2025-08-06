namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Menu;
    using static PizzaApp.Services.Common.ExceptionMessages;
    public class PizzaMenuService : IPizzaMenuService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IIngredientsService _pizzaIngredientsService;

        public PizzaMenuService(IPizzaRepository pizzaRepository, 
            IIngredientsService pizzaIngredientsService)
        {
            this._pizzaRepository = pizzaRepository;
            this._pizzaIngredientsService = pizzaIngredientsService;
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllBaseItemsAsync()
        {
            IEnumerable<Pizza> allPizzas = await this._pizzaRepository
                .DisableTracking()
                .GetAllBasePizzasAsync();

            return allPizzas
                .Select(p => new MenuItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Category = MenuCategory.Pizzas
                });
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllUserPizzasAsync(Guid userId)
        {
            IEnumerable<Pizza> allUserPizzas = await this._pizzaRepository
                .DisableTracking()
                .GetAllUserPizzasAsync(userId);

            return allUserPizzas
                .Select(p => new MenuItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Category = MenuCategory.Pizzas
                });
        }

        public async Task<PizzaDetailsViewWrapper> GetPizzaDetailsByIdAsync(int id)
        {
            CustomizePizzaInputModel pizzaDetails = await this.GetPizzaDetailsViewModelByIdAsync(id)
                ?? throw new EntityNotFoundException(EntityNotFoundMessage, nameof(Pizza), id);

            PizzaDetailsViewWrapper orderPizzaView = new()
            {
                Pizza = pizzaDetails,
                Ingredients = await this._pizzaIngredientsService.GetAllIngredientsAsync()
            };

            return orderPizzaView;
        }

        private async Task<CustomizePizzaInputModel?> GetPizzaDetailsViewModelByIdAsync(int id)
        {
            Pizza? pizza = await this._pizzaRepository
                                     .DisableTracking()
                                     .GetByIdWithIngredientsAsync(id);

            if (pizza is null)
                return null;

            return new CustomizePizzaInputModel
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Description = pizza.Description,
                DoughId = pizza.Dough.Id,
                SauceId = pizza.SauceId,
                ImageUrl = pizza.ImageUrl,
                Price = pizza.Dough.Price
                      + (pizza.Sauce == null ? 0 : pizza.Sauce.Price)
                      + pizza.Toppings.Sum(t => t.Topping.Price),
                SelectedToppingIds = pizza.Toppings.Select(t => t.ToppingId).ToList()
            };
        }
    }
}
