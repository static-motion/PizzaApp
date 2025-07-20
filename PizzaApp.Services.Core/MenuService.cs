namespace PizzaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
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
        private readonly IToppingRepository _toppingRepository;

        private Dictionary<MenuCategory, Func<Task<IEnumerable<MenuItemViewModel>>>> _menuCategoryMethodLookup;

        public MenuService(IPizzaRepository pizzaRepository, 
            IDrinkRepository drinkRepository, 
            IDessertRepository dessertRepository,
            ISauceRepository sauceRepository,
            IDoughRepository doughRepository,
            IToppingRepository toppingRepository)
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
                ToppingCategories = allToppingsByCategories,
                Doughs = allDoughs,
                Sauces = allSauces
            };

            return orderPizzaView;
        }

        private Task<PizzaDetailsViewModel?> GetPizzaDetailsViewModelByIdAsync(int id)
        {
            return this._pizzaRepository
                .GetAllAttached()
                .Where(p => p.Id == id)
                .Select(p => new PizzaDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    DoughId = p.Dough.Id,
                    SauceId = p.SauceId,
                    ImageUrl = p.ImageUrl,
                    Price = p.Dough.Price
                          + (p.Sauce == null ? 0 : p.Sauce.Price)
                          + p.Toppings.Sum(t => t.Topping.Price),
                    SelectedToppingIds = p.Toppings.Select(t => t.ToppingId).ToList()
                })
                .FirstOrDefaultAsync();
        }

        private Task<List<SauceViewModel>> GetAllSaucesAsync()
        {
            return this._sauceRepository
                .GetAllAttached()
                .Select(s => new SauceViewModel
                {
                    Id = s.Id,
                    Name = s.Type,
                    Price = s.Price
                })
                .ToListAsync();
        }

        private Task<List<DoughViewModel>> GetAllDoughsAsync()
        {
            return this._doughRepository
                .GetAllAttached()
                .Select(d => new DoughViewModel
                {
                    Id = d.Id,
                    Name = d.Type,
                    Price = d.Price
                })
                .ToListAsync();
        }

        private Task<List<ToppingCategoryViewModel>> GetAllCategoriesWithToppingsAsync()
        {
            return this._toppingRepository
                .GetAllAttached()
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
                })
                .ToListAsync();
        }
    }
}
