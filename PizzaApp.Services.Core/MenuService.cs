﻿namespace PizzaApp.Services.Core
{
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
        private readonly IPizzaIngredientsService _pizzaIngredientsService;

        private Dictionary<MenuCategory, Func<Task<IEnumerable<MenuItemViewModel>>>> _menuCategoryMethodLookup;

        public MenuService(IPizzaRepository pizzaRepository, 
            IDrinkRepository drinkRepository, 
            IDessertRepository dessertRepository,
            IPizzaIngredientsService pizzaIngredientsService)
        {
            this._pizzaRepository = pizzaRepository;
            this._drinkRepository = drinkRepository;
            this._dessertRepository = dessertRepository;
            this._pizzaIngredientsService = pizzaIngredientsService;

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

        public async Task<OrderPizzaViewWrapper?> GetPizzaDetailsByIdAsync(int id)
        {
            PizzaDetailsViewModel? pizzaDetails = await this.GetPizzaDetailsViewModelByIdAsync(id);

            // return early if pizza does not exist
            if (pizzaDetails is null)
                return null;

            IReadOnlyList<ToppingCategoryViewWrapper> allToppingsByCategories = await this._pizzaIngredientsService.GetAllCategoriesWithToppingsAsync();
            IReadOnlyList<DoughViewModel> allDoughs = await this._pizzaIngredientsService.GetAllDoughsAsync(disableTracking: true);
            IReadOnlyList<SauceViewModel> allSauces = await this._pizzaIngredientsService.GetAllSaucesAsync(disableTracking: true);

            // create the model
            OrderPizzaViewWrapper orderPizzaView = new()
            {
                Pizza = pizzaDetails,
                Ingredients = new PizzaIngredientsViewWrapper
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

        public async Task<OrderItemViewModel?> GetOrderItemDetailsAsync(int id, MenuCategory? category)
        {
            return category switch
            {
                MenuCategory.Drinks => await this.GetDrinkById(id),
                MenuCategory.Desserts => await this.GetDessertById(id),
                _ => null
            };
        }

        private async Task<OrderItemViewModel?> GetDrinkById(int id)
        {
            Drink? drink = await this._drinkRepository.GetByIdAsync(id);

            if (drink is null)
                return null;

            return new OrderItemViewModel()
            {
                Id = drink.Id,
                Name = drink.Name,
                Category = MenuCategory.Drinks,
                Description = drink.Description,
                ImageUrl = drink.ImageUrl,
                Price = drink.Price
            };
        }

        private async Task<OrderItemViewModel?> GetDessertById(int id)
        {
            Dessert? dessert = await this._dessertRepository.GetByIdAsync(id);

            if (dessert is null)
                return null;

            return new OrderItemViewModel()
            {
                Id = dessert.Id,
                Name = dessert.Name,
                Category = MenuCategory.Desserts,
                Description = dessert.Description,
                ImageUrl = dessert.ImageUrl,
                Price = dessert.Price
            };
        }
    }
}
