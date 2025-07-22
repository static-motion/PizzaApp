namespace PizzaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using PizzaApp.Web.ViewModels.Menu;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Threading.Tasks;

    public class MenuService : IMenuService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IDrinkRepository _drinkRepository;
        private readonly IDessertRepository _dessertRepository;
        private readonly ISauceRepository _sauceRepository;
        private readonly IDoughRepository _doughRepository;
        private readonly IToppingCategoryRepository _toppingRepository;

        private Dictionary<MenuCategory, Func<Task<IEnumerable<MenuItemViewModel>>>> _menuCategoryMethodLookup;

        public MenuService(IPizzaRepository pizzaRepository, 
            IDrinkRepository drinkRepository, 
            IDessertRepository dessertRepository,
            ISauceRepository sauceRepository,
            IDoughRepository doughRepository,
            IToppingCategoryRepository toppingRepository)
        {
            this._pizzaRepository = pizzaRepository;
            this._drinkRepository = drinkRepository;
            this._dessertRepository = dessertRepository;
            this._sauceRepository = sauceRepository;
            this._doughRepository = doughRepository;
            this._toppingRepository = toppingRepository;

            // new menu categories should be added here
            // TODO: add pasta, salads and whatver else I can think of
            _menuCategoryMethodLookup = new()
            {
                [MenuCategory.Pizzas] = this.GetAllPizzasForMenuAsync,
                [MenuCategory.Drinks] = this.GetAllDrinksForMenuAsync,
                [MenuCategory.Desserts] = this.GetAllDessertsForMenuAsync,
            };
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllPizzasForMenuAsync()
        {
            IEnumerable<Pizza> allPizzas = await this._pizzaRepository.GetAllBasePizzasAsync();
            
            return allPizzas
                .Select(p => new MenuItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl

                });
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllDrinksForMenuAsync()
        {
            IEnumerable<Drink> allDrinks = await this._drinkRepository.GetAllAsync();
            
            return allDrinks
                .Select(d => new MenuItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl

                });
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllDessertsForMenuAsync()
        {
            IEnumerable<Dessert> allDesserts = await this._dessertRepository.GetAllAsync();

            return allDesserts
                .Select(d => new MenuItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl

                });
        }

        public async Task<IReadOnlyCollection<MenuItemViewModel>> GetAllMenuItemsForCategoryAsync(MenuCategory category)
        {
            IReadOnlyCollection<MenuItemViewModel> menuItems;

            // check if the category exists in the lookup
            if (!this._menuCategoryMethodLookup.TryGetValue(category, out var method))
            {
                return [];
            }

            // invoke the method to get the menu items
            menuItems = (await method()).ToImmutableList();

            // attach the category to each item
            foreach (MenuItemViewModel item in menuItems)
            {
                item.Category = category;
            }

            return menuItems;
        }

        public async Task<OrderPizzaViewModel?> GetPizzaDetailsByIdAsync(int id)
        {
            PizzaDetailsViewModel? pizzaDetails = await this.GetPizzaDetailsViewModelByIdAsync(id);

            // return early if pizza does not exist
            if (pizzaDetails is null)
                return null;

            IReadOnlyList<ToppingCategoryViewModel> allToppingsByCategories = await this.GetAllCategoriesWithToppingsAsync();
            IReadOnlyList<DoughViewModel> allDoughs = await this.GetAllDoughsAsync();
            IReadOnlyList<SauceViewModel> allSauces = await this.GetAllSaucesAsync();

            // create the model
            OrderPizzaViewModel orderPizzaView = new()
            {
                Pizza = pizzaDetails,
                Ingredients = new PizzaIngredientsViewModel
                {
                    ToppingCategories = allToppingsByCategories,
                    Doughs = allDoughs,
                    Sauces = allSauces
                }
            };

            return orderPizzaView;
        }

        private async Task<PizzaDetailsViewModel?> GetPizzaDetailsViewModelByIdAsync(int id)
        {
            Pizza? pizza = await this._pizzaRepository.GetByIdWithIngredientsAsync(id);
            if (pizza is null)
                return null;

            return new PizzaDetailsViewModel
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

        private async Task<List<SauceViewModel>> GetAllSaucesAsync()
        {
            IEnumerable<Sauce> allSauces = await this._sauceRepository.GetAllAsync(asNoTracking: true);
                
                return allSauces.Select(s => new SauceViewModel
                {
                    Id = s.Id,
                    Name = s.Type,
                    Price = s.Price
                })
                .ToList();
        }

        private async Task<List<DoughViewModel>> GetAllDoughsAsync()
        {
            IEnumerable<Dough> allDoughs = await this._doughRepository.GetAllAsync(asNoTracking: true);

            return allDoughs
                .Select(d => new DoughViewModel
                {
                    Id = d.Id,
                    Name = d.Type,
                    Price = d.Price
                })
                .ToList();
        }

        private async Task<List<ToppingCategoryViewModel>> GetAllCategoriesWithToppingsAsync()
        {
            IEnumerable<ToppingCategory> allToppingCategories = await this._toppingRepository
                .GetAllWithToppingsAsync(asNoTracking: true);
            
            return allToppingCategories
                .Select(tc => new ToppingCategoryViewModel
                {
                    Id = tc.Id,
                    Name = tc.Name,
                    Toppings = tc.Toppings.Select(t => new ToppingViewModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Price = t.Price
                    }).ToList()
                }).ToList();
        }
    }
}
