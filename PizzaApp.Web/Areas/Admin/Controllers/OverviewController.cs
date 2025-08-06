namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Data.Models;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class OverviewController : BaseAdminController
    {
        private readonly IPizzaManagementService _pizzaManagementService;
        private readonly IToppingManagementService _toppingManagementService;
        private readonly IIngredientsService _pizzaIngredientsService;
        private readonly IDoughManagementService _doughManagementService;
        private readonly ISauceManagementService _sauceManagementService;
        private readonly IToppingCategoryManagementService _toppingCategoryManagementService;

        private const string PizzaEntityName = nameof(Pizza);
        private const string DoughEntityName = nameof(Dough);
        private const string SauceEntityName = nameof(Sauce);
        private const string ToppingEntityName = nameof(Topping);
        private const string ToppingCategoryEntityName = nameof(ToppingCategory);
        private const int PageSize = 10;

        public OverviewController(IPizzaManagementService pizzaManagementService, 
            IToppingManagementService toppingManagementService,
            IIngredientsService pizzaIngredientsService,
            IDoughManagementService doughManagementService,
            ISauceManagementService sauceManagementService,
            IToppingCategoryManagementService toppingCategoryManagementService)
        {
            this._pizzaManagementService = pizzaManagementService;
            this._toppingManagementService = toppingManagementService;
            this._pizzaIngredientsService = pizzaIngredientsService;
            this._doughManagementService = doughManagementService;
            this._sauceManagementService = sauceManagementService;
            this._toppingCategoryManagementService = toppingCategoryManagementService;
        }

        private int CalculatePageCount(int totalItems) 
            => (int)Math.Ceiling((double)totalItems / PageSize);

        public IActionResult Index()
        {
            return this.RedirectToAction(nameof(Pizzas));
        }

        [HttpGet]
        public async Task<IActionResult> Pizzas(int page = 1)
        {
            (int totalItems, IEnumerable<MenuItemViewModel> pagePizzas) 
                = await this._pizzaManagementService.GetPizzasOverviewPaginatedAsync(page, PageSize);

            int pageCount = this.CalculatePageCount(totalItems);

            this.ViewData["EntityName"] = PizzaEntityName;
            this.ViewData["ControllerName"] = nameof(Pizzas);

            AdminItemsOverviewViewWrapper view = new()
            {
                Category = "Pizzas",
                CurrentPage = page,
                Items = pagePizzas,
                TotalPages = pageCount
            };

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> Doughs(int page = 1)
        {
            (int totalItems, IEnumerable<MenuItemViewModel> pageDoughs)
                = await this._doughManagementService.GetDoughsOverviewPaginatedAsync(page, PageSize);

            int pageCount = this.CalculatePageCount(totalItems);

            this.ViewData["EntityName"] = DoughEntityName;
            this.ViewData["ControllerName"] = nameof(Doughs);


            AdminItemsOverviewViewWrapper view = new()
            {
                Category = "Doughs",
                CurrentPage = page,
                Items = pageDoughs,
                TotalPages = pageCount
            };

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> Sauces(int page = 1)
        {
            (int totalItems, IEnumerable<MenuItemViewModel> pageSauces)
                = await this._sauceManagementService.GetSaucesOverviewPaginatedAsync(page, PageSize);

            int pageCount = this.CalculatePageCount(totalItems);

            this.ViewData["EntityName"] = SauceEntityName;
            this.ViewData["ControllerName"] = nameof(Sauces);

            AdminItemsOverviewViewWrapper view = new()
            {
                Category = "Sauces",
                CurrentPage = page,
                Items = pageSauces,
                TotalPages = pageCount
            };

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> Toppings(int page = 1)
        {
            (int totalItems, IEnumerable<MenuItemViewModel> pageToppings)
                = await this._toppingManagementService.GetToppingsOverviewPaginatedAsync(page, PageSize);

            int pageCount = this.CalculatePageCount(totalItems);

            this.ViewData["EntityName"] = ToppingEntityName;
            this.ViewData["ControllerName"] = nameof(Toppings);

            AdminItemsOverviewViewWrapper view = new()
            {
                Category = "Toppings",
                CurrentPage = page,
                Items = pageToppings,
                TotalPages = pageCount
            };

            return this.View("Items", view);
        }

        

        [HttpGet]
        public async Task<IActionResult> ToppingCategories(int page = 1)
        {
            (int totalItems, IEnumerable<MenuItemViewModel> pageToppingCategories)
                = await this._toppingCategoryManagementService
                .GetToppingCategoriesOverviewPaginatedAsync(page, PageSize);

            int pageCount = this.CalculatePageCount(totalItems);

            this.ViewData["EntityName"] = ToppingCategoryEntityName;
            this.ViewData["ControllerName"] = nameof(ToppingCategories);

            AdminItemsOverviewViewWrapper view = new()
            {
                Category = "Topping Categories",
                CurrentPage = page,
                Items = pageToppingCategories,
                TotalPages = pageCount
            };

            return this.View("Items", view);
        }
    }
}
