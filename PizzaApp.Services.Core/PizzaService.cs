namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using PizzaApp.Web.ViewModels.Menu;
    using PizzaApp.Web.ViewModels.Pizzas;

    public class PizzaService : IPizzaService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IToppingCategoryRepository _toppingRepository;
        private readonly IDoughRepository _doughRepository;
        private readonly ISauceRepository _sauceRepository;

        public PizzaService(IPizzaRepository pizzaRepository, 
            IToppingCategoryRepository toppingCategoryRepository,
            IDoughRepository doughRepository,
            ISauceRepository sauceRepository)
        {
            this._pizzaRepository = pizzaRepository;
            this._toppingRepository = toppingCategoryRepository;
            this._doughRepository = doughRepository;
            this._sauceRepository = sauceRepository;
        }

        public async Task<PizzaIngredientsViewModel> GetAllIngredientsAsync()
        {
            IReadOnlyList<ToppingCategoryViewModel> allToppingsByCategories = await this.GetAllCategoriesWithToppingsAsync();
            IReadOnlyList<DoughViewModel> allDoughs = await this.GetAllDoughsAsync();
            IReadOnlyList<SauceViewModel> allSauces = await this.GetAllSaucesAsync();

            // create the model
            PizzaIngredientsViewModel ingredients = new()
            {
                ToppingCategories = allToppingsByCategories,
                Doughs = allDoughs,
                Sauces = allSauces
            };

            return ingredients;
        }

        // TODO: the same method is used in MenuService. Refactor to a common service?
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

        public async Task<bool> CreatePizzaAsync(PizzaInputModel pizza, IEnumerable<int> selectedToppingIds, Guid userId)
        {
            IEnumerable<Topping> toppings = await this._toppingRepository.GetAllToppingsFromRangeAsync(selectedToppingIds);
            
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
