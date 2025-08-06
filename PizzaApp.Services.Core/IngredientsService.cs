namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class IngredientsService : IIngredientsService
    {
        private readonly ISauceRepository _sauceRepository;
        private readonly IDoughRepository _doughRepository;
        private readonly IToppingCategoryRepository _toppingRepository;
        public IngredientsService(ISauceRepository sauceRepository, 
            IDoughRepository doughRepository,
            IToppingCategoryRepository toppingRepository)
        {
            this._toppingRepository = toppingRepository;
            this._sauceRepository = sauceRepository;
            this._doughRepository = doughRepository;
        }
        public async Task<IReadOnlyList<SauceViewModel>> GetAllSaucesAsync(bool ignoreFiltering = false, bool disableTracking = false)
        {
            if (ignoreFiltering)
                this._sauceRepository.IgnoreFiltering();

            if (disableTracking)
                this._sauceRepository.DisableTracking();

            IEnumerable<Sauce> allSauces = await this._sauceRepository
                .GetAllAsync();

            return allSauces.Select(s => new SauceViewModel
            {
                Id = s.Id,
                Name = s.Type,
                Price = s.Price,
                IsActive = s.IsDeleted == false

            })
            .ToList();
        }

        public async Task<IReadOnlyList<DoughViewModel>> GetAllDoughsAsync(bool ignoreFiltering = false, bool disableTracking = false)
        {
            if (ignoreFiltering)
                this._doughRepository.IgnoreFiltering();

            if (disableTracking)
                this._doughRepository.DisableTracking();

            IEnumerable<Dough> allDoughs = await this._doughRepository
                .GetAllAsync();

            return allDoughs
                .Select(d => new DoughViewModel
                {
                    Id = d.Id,
                    Name = d.Type,
                    Price = d.Price,
                    IsActive = d.IsDeleted == false
                })
                .ToList();
        }

        public async Task<IReadOnlyList<ToppingCategoryViewWrapper>> GetAllCategoriesWithToppingsAsync(bool ignoreFiltering = false, bool disableTracking = false)
        {
            if (ignoreFiltering)
                this._toppingRepository.IgnoreFiltering();

            if (disableTracking)
                this._toppingRepository.DisableTracking();


            IEnumerable<ToppingCategory> allToppingCategories = await this._toppingRepository
                .GetAllCategoriesWithToppingsAsync();

            return allToppingCategories
                .Select(tc => new ToppingCategoryViewWrapper
                {
                    Id = tc.Id,
                    Name = tc.Name,
                    IsActive = tc.IsDeleted == false,
                    Toppings = tc.Toppings.Select(t => new ToppingViewModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Price = t.Price,
                        IsActive = t.IsDeleted == false && tc.IsDeleted == false
                    }).ToList()
                }).ToList();
        }

        public async Task<PizzaIngredientsViewWrapper> GetAllIngredientsAsync(bool ignoreFiltering = false, bool disaleTracking = false)
        {
            IReadOnlyList<ToppingCategoryViewWrapper> allToppingsByCategories 
                = await this.GetAllCategoriesWithToppingsAsync(ignoreFiltering, disaleTracking);

            IReadOnlyList<DoughViewModel> allDoughs 
                = await this.GetAllDoughsAsync(ignoreFiltering, disaleTracking);

            IReadOnlyList<SauceViewModel> allSauces 
                = await this.GetAllSaucesAsync(ignoreFiltering, disaleTracking);

            // create the model
            PizzaIngredientsViewWrapper ingredients = new()
            {
                ToppingCategories = allToppingsByCategories,
                Doughs = allDoughs,
                Sauces = allSauces
            };

            return ingredients;
        }
    }
}
