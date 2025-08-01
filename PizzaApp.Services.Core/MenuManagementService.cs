namespace PizzaApp.Services.Core
{
    using Data.Models.MappingEntities;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using PizzaApp.Web.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    //TODO: Refactor into multiple management services
    //and use service composition
    public class MenuManagementService : IMenuManagementService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IPizzaIngredientsService _pizzaIngredientsService;
        private readonly IDoughRepository _doughRepository;
        private readonly ISauceRepository _sauceRepository;
        private readonly IToppingRepository _toppingRepository;
        private readonly IToppingCategoryRepository _toppingCategoryRepository;

        private readonly Dictionary<ManagementCategory, Func<Task<IEnumerable<MenuItemViewModel>>>> _categoryItemFetchers;

        public MenuManagementService(IPizzaRepository pizzaRepository,
            IPizzaIngredientsService pizzaIngredientsService,
            IDoughRepository doughRepository,
            ISauceRepository sauceRepository,
            IToppingRepository toppingRepository,
            IToppingCategoryRepository toppingCategoryRepository)
        {
            this._pizzaRepository = pizzaRepository;
            this._pizzaIngredientsService = pizzaIngredientsService;
            this._doughRepository = doughRepository;
            this._sauceRepository = sauceRepository;
            this._toppingRepository = toppingRepository;
            this._toppingCategoryRepository = toppingCategoryRepository;


            _categoryItemFetchers = new()
            {
                [ManagementCategory.Pizza] = this.GetAllPizzas,
                [ManagementCategory.Dough] = this.GetAllDoughs,
                [ManagementCategory.Sauce] = this.GetAllSauces,
                [ManagementCategory.Topping] = this.GetAllToppings,
                [ManagementCategory.ToppingCategory] = this.GetAllToppingCategories
            };
        }

        private async Task<IEnumerable<MenuItemViewModel>> GetAllToppingCategories()
        {
            IEnumerable<ToppingCategory> toppingCategories = await this._toppingCategoryRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetAllAsync();

            return toppingCategories.Select(d => new MenuItemViewModel
            {
                Id = d.Id,
                IsActive = !d.IsDeleted,
                Name = d.Name
            });
        }

        private async Task<IEnumerable<MenuItemViewModel>> GetAllToppings()
        {
            IEnumerable<Topping> toppings = await this._toppingRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetAllAsync();

            return toppings.Select(d => new MenuItemViewModel
            {
                Id = d.Id,
                IsActive = !d.IsDeleted,
                Name = d.Name
            });
        }

        private async Task<IEnumerable<MenuItemViewModel>> GetAllDoughs()
        {
            IEnumerable<Dough> doughs = await this._doughRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetAllAsync();

            return doughs.Select(d => new MenuItemViewModel
            {
                Id = d.Id,
                IsActive = !d.IsDeleted,
                Name = d.Type
            });
        }

        private async Task<IEnumerable<MenuItemViewModel>> GetAllSauces()
        {
            IEnumerable<Sauce> sauces = await this._sauceRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetAllAsync();

            return sauces.Select(d => new MenuItemViewModel
            {
                Id = d.Id,
                IsActive = !d.IsDeleted,
                Name = d.Type
            });
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllItemsFromCategory(ManagementCategory category)
        {
            return await this._categoryItemFetchers[category]();
        }

        public async Task<EditAdminPizzaViewWrapper?> GetPizzaDetailsByIdAsync(int id)
        {
            AdminPizzaInputModel? pizzaDetails = await this.GetPizzaDetailsViewModelByIdAsync(id);

            // return early if pizza does not exist
            if (pizzaDetails is null)
                return null;

            IReadOnlyList<ToppingCategoryViewWrapper> allToppingsByCategories 
                = await this._pizzaIngredientsService.GetAllCategoriesWithToppingsAsync(ignoreFiltering: true, disableTracking: true);
            IReadOnlyList<DoughViewModel> allDoughs 
                = await this._pizzaIngredientsService.GetAllDoughsAsync(ignoreFiltering: true, disableTracking: true);
            IReadOnlyList<SauceViewModel> allSauces 
                = await this._pizzaIngredientsService.GetAllSaucesAsync(ignoreFiltering: true, disableTracking: true);

            // create the model
            EditAdminPizzaViewWrapper managePizzaView = new()
            {
                Pizza = pizzaDetails,
                Ingredients = new PizzaIngredientsViewWrapper
                {
                    ToppingCategories = allToppingsByCategories,
                    Doughs = allDoughs,
                    Sauces = allSauces
                }
            };

            return managePizzaView;
        }

        private async Task<AdminPizzaInputModel?> GetPizzaDetailsViewModelByIdAsync(int id)
        {
            Pizza? pizza = await this._pizzaRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetByIdWithIngredientsAsync(id);
            if (pizza is null)
                return null; // TODO: change method signature and throw exception instead

            return new AdminPizzaInputModel
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
                SelectedToppingIds = pizza.Toppings.Select(t => t.ToppingId).ToHashSet(),
                IsDeleted = pizza.IsDeleted,
            };

        }

        private async Task<IEnumerable<MenuItemViewModel>> GetAllPizzas()
        {
            IEnumerable<Pizza> pizzas = await this._pizzaRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetAllBasePizzasWithIngredientsAsync();

            var firstSauce = pizzas.First().Sauce;
            var allMatch = pizzas.All(p => ReferenceEquals(p.Sauce, firstSauce));

            return pizzas.Select(p => new MenuItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                IsActive = p.IsDeleted == false // the pizza must be active
                    && (p.Sauce == null || p.Sauce.IsDeleted == false) // the sauce must be either not set or active
                    && p.Dough.IsDeleted == false // the dough must be active
                    && p.Toppings.All(t => t.Topping.IsDeleted == false && t.Topping.ToppingCategory.IsDeleted == false) // all toppings must be active
            });

        }

        public async Task EditPizzaAsync(AdminPizzaInputModel inputModel)
        {
            Pizza? pizzaToEdit = await this._pizzaRepository.IgnoreFiltering().GetByIdWithIngredientsAsync(inputModel.Id);

            if (pizzaToEdit is null)
            {
                throw new InvalidOperationException(); // TODO: Add error message
            }

            pizzaToEdit.Name = inputModel.Name;
            pizzaToEdit.DoughId = inputModel.DoughId;
            pizzaToEdit.SauceId = inputModel.SauceId;
            pizzaToEdit.Description = inputModel.Description;
            pizzaToEdit.ImageUrl = inputModel.ImageUrl;
            pizzaToEdit.IsDeleted = inputModel.IsDeleted;
            pizzaToEdit.Toppings = inputModel.SelectedToppingIds
                                        .Select(id => new PizzaTopping()
                                        {
                                            ToppingId = id
                                        })
                                        .ToList();

            await this._pizzaRepository.SaveChangesAsync();
        }

        public async Task<EditDoughInputModel> GetDoughDetailsByIdAsync(int id)
        {
            Dough? dough = await this._doughRepository
                .IgnoreFiltering()
                .GetByIdAsync(id) // TODO: Move this exception message to ErrorMessages
                ?? throw new InvalidOperationException($"Dough with ID: {id} not found.");

            return new EditDoughInputModel
            {
                Id = dough.Id,
                Description = dough.Description,
                Price = dough.Price,
                Type = dough.Type,
                IsDeleted = dough.IsDeleted
            };
        }

        public async Task EditDoughAsync(EditDoughInputModel model)
        {
            Dough? dough = await this._doughRepository
                .IgnoreFiltering()
                .GetByIdAsync(model.Id)
                ?? throw new InvalidOperationException(); //TODO: Add exception message

            dough.Type = model.Type;
            dough.Price = model.Price;
            dough.Description = model.Description;
            dough.IsDeleted = model.IsDeleted;

            this._doughRepository.Update(dough);
            await this._doughRepository.SaveChangesAsync();
        }

        public async Task EditSauceAsync(EditSauceInputModel inputSauce)
        {
            Sauce? sauce = await this._sauceRepository
                .IgnoreFiltering()
                .GetByIdAsync(inputSauce.Id)
                ?? throw new InvalidOperationException(); //TODO: Add exception message

            sauce.Type = inputSauce.Type;
            sauce.Price = inputSauce.Price;
            sauce.Description = inputSauce.Description;
            sauce.IsDeleted = inputSauce.IsDeleted;

            this._sauceRepository.Update(sauce);
            await this._sauceRepository.SaveChangesAsync();
        }

        public async Task<EditSauceInputModel> GetSauceDetailsByIdAsync(int id)
        {
            Sauce? sauce = await this._sauceRepository
                .IgnoreFiltering()
                .DisableTracking()
                .GetByIdAsync(id)
            ?? throw new InvalidOperationException();

            return new EditSauceInputModel
            {
                Id = sauce.Id,
                Description = sauce.Description,
                IsDeleted = sauce.IsDeleted,
                Type = sauce.Type,
                Price = sauce.Price
            };
        }

        public async Task<EditToppingViewWrapper> GetToppingDetailsByIdAsync(int id)
        {
            Topping? topping = await this._toppingRepository
                .IgnoreFiltering()
                .DisableTracking()
                .GetByIdAsync(id)
            ?? throw new InvalidOperationException(); //TODO:

            IEnumerable<ToppingCategory> toppingCategories
                = await this._toppingCategoryRepository
                            .DisableTracking()
                            .IgnoreFiltering()
                            .GetAllAsync();

            return new EditToppingViewWrapper
            {
                AllCategories = toppingCategories.Select(tc => new ToppingCategorySelectViewModel
                {
                    Id = tc.Id,
                    Name = tc.Name,
                    IsActive = tc.IsDeleted == false
                }),
                ToppingInputModel = new EditToppingInputModel
                { 
                    Id = topping.Id,
                    Description = topping.Description,
                    Name = topping.Name,
                    ToppingCategoryId = topping.ToppingCategoryId,
                    IsDeleted = topping.IsDeleted,
                    Price = topping.Price
                }
            };
        }

        public async Task EditToppingAsync(EditToppingInputModel inputTopping)
        {
            Topping? topping = await this._toppingRepository
                .IgnoreFiltering()
                .GetByIdAsync(inputTopping.Id)
            ?? throw new InvalidOperationException();

            bool categoryExists = await this._toppingCategoryRepository
                .IgnoreFiltering()
                .ExistsAsync(tc => tc.Id == inputTopping.ToppingCategoryId);

            if (!categoryExists)
                throw new InvalidOperationException();

            topping.Name = inputTopping.Name;
            topping.Description = inputTopping.Description;
            topping.IsDeleted = inputTopping.IsDeleted;
            topping.Price = inputTopping.Price;
            topping.ToppingCategoryId = inputTopping.ToppingCategoryId;

            this._toppingRepository.Update(topping);
            await this._toppingRepository.SaveChangesAsync();
        }

        public async Task<EditToppingCategoryInputModel> GetToppingCategoryDetailsByIdAsync(int id)
        {
            ToppingCategory? category = await this._toppingCategoryRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetByIdAsync(id)
            ?? throw new InvalidOperationException();

            return new EditToppingCategoryInputModel
            {
                Id = category.Id,
                Name = category.Name,
                IsDeleted = category.IsDeleted,
            };
        }

        public async Task EditToppingCategoryAsync(EditToppingCategoryInputModel inputModel)
        {
            ToppingCategory? toppingCategory
                = await this._toppingCategoryRepository
                            .IgnoreFiltering()
                            .GetByIdAsync(inputModel.Id)
            ?? throw new InvalidOperationException(); // TODO

            toppingCategory.Name = inputModel.Name;
            toppingCategory.IsDeleted = inputModel.IsDeleted;

            this._toppingCategoryRepository.Update(toppingCategory);
            await this._toppingCategoryRepository.SaveChangesAsync();
        }
    }
}