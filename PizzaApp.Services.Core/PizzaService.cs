﻿namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using PizzaApp.Web.ViewModels.Pizzas;

    public class PizzaService : IPizzaService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IToppingCategoryRepository _toppingCategoryRepository;
        private readonly IDoughRepository _doughRepository;
        private readonly ISauceRepository _sauceRepository;
        private readonly IToppingRepository _toppingRepository;

        public PizzaService(IPizzaRepository pizzaRepository, 
            IToppingCategoryRepository toppingCategoryRepository,
            IDoughRepository doughRepository,
            ISauceRepository sauceRepository,
            IToppingRepository toppingRepository)
        {
            this._pizzaRepository = pizzaRepository;
            this._toppingCategoryRepository = toppingCategoryRepository;
            this._doughRepository = doughRepository;
            this._sauceRepository = sauceRepository;
            this._toppingRepository = toppingRepository;
        }

        public async Task<PizzaIngredientsViewWrapper> GetAllIngredientsAsync()
        {
            IReadOnlyList<ToppingCategoryViewWrapper> allToppingsByCategories = await this.GetAllCategoriesWithToppingsAsync();
            IReadOnlyList<DoughViewModel> allDoughs = await this.GetAllDoughsAsync();
            IReadOnlyList<SauceViewModel> allSauces = await this.GetAllSaucesAsync();

            // create the model
            PizzaIngredientsViewWrapper ingredients = new()
            {
                ToppingCategories = allToppingsByCategories,
                Doughs = allDoughs,
                Sauces = allSauces
            };

            return ingredients;
        }

        // TODO: the same method is used in MenuService. Refactor to a common service?
        private async Task<List<ToppingCategoryViewWrapper>> GetAllCategoriesWithToppingsAsync()
        {
            IEnumerable<ToppingCategory> allToppingCategories = await this._toppingCategoryRepository
                .DisableTracking()
                .GetAllWithToppingsAsync();

            return allToppingCategories
                .Select(tc => new ToppingCategoryViewWrapper
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

        private async Task<List<SauceViewModel>> GetAllSaucesAsync()
        {
            IEnumerable<Sauce> allSauces = await this._sauceRepository
                .DisableTracking()
                .GetAllAsync();

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
            IEnumerable<Dough> allDoughs = await this._doughRepository
                .DisableTracking()
                .GetAllAsync();

            return allDoughs
                .Select(d => new DoughViewModel
                {
                    Id = d.Id,
                    Name = d.Type,
                    Price = d.Price
                })
                .ToList();
        }

        public async Task<bool> CreatePizzaAsync(PizzaInputModel pizza, IEnumerable<int> selectedToppingIds, Guid userId)
        {
            IEnumerable<Topping> toppings = await this._toppingRepository
                .DisableTracking()
                .GetRangeByIdsAsync(selectedToppingIds);
                //.GetAllToppingsFromRangeAsync(selectedToppingIds);
            
            bool doughExists = await this._doughRepository.ExistsAsync(d => d.Id == pizza.DoughId);
            bool sauceExists = await this._sauceRepository.ExistsAsync(s => s.Id == pizza.SauceId);

            if (!doughExists || !sauceExists)
            {
                // Invalid dough or sauce ID
                return false;
            }

            if (toppings.Count() != selectedToppingIds.Count())
            {
                // Not all selected toppings are valid
                return false;
            }

            // Pizza and userId should be valid at this point because
            // they went through model validation in the controller
            Pizza newPizza = new()
            {
                Name = pizza.Name,
                Description = pizza.Description,
                DoughId = pizza.DoughId,
                SauceId = pizza.SauceId,
                CreatorUserId = userId,
                PizzaType = pizza.PizzaType
            };

            foreach (Topping topping in toppings)
            {
                newPizza.Toppings.Add(new PizzaTopping
                {
                    Topping = topping,
                    ToppingId = topping.Id,
                    Pizza = newPizza
                });
            }

            await this._pizzaRepository.AddAsync(newPizza);
            await this._pizzaRepository.SaveChangesAsync();
            return true;
        }
    }
}
