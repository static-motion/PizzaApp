namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PizzaIngredientsService : IPizzaIngredientsService
    {
        private readonly ISauceRepository _sauceRepository;
        private readonly IDoughRepository _doughRepository;
        private readonly IToppingCategoryRepository _toppingRepository;
        public PizzaIngredientsService(ISauceRepository sauceRepository, 
            IDoughRepository doughRepository,
            IToppingCategoryRepository toppingRepository)
        {
            this._toppingRepository = toppingRepository;
            this._sauceRepository = sauceRepository;
            this._doughRepository = doughRepository;
        }
        public async Task<IReadOnlyList<SauceViewModel>> GetAllSaucesAsync()
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

        public async Task<IReadOnlyList<DoughViewModel>> GetAllDoughsAsync()
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

        public async Task<IReadOnlyList<ToppingCategoryViewModel>> GetAllCategoriesWithToppingsAsync()
        {
            IEnumerable<ToppingCategory> allToppingCategories = await this._toppingRepository
                .DisableTracking()
                .GetAllWithToppingsAsync();

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
