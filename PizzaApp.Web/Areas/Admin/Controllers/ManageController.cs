namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class ManageController : BaseAdminController
    {
        private readonly IMenuManagementService _menuManagementService;
        private readonly IMenuService _menuService;

        public ManageController(IMenuManagementService menuManagementService,
            IMenuService menuService)
        {
            this._menuManagementService = menuManagementService;
            this._menuService = menuService;
        }
        public IActionResult Index()
        {
            return this.RedirectToAction(nameof(Pizzas));
        }

        [HttpGet]
        public async Task<IActionResult> Pizzas()
        {
            ManagementCategory category = ManagementCategory.Pizza;

            AdminItemsOverviewViewModel view = await this.CreateItemsOverviewModel(category);

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> Doughs()
        {
            ManagementCategory category = ManagementCategory.Dough;

            AdminItemsOverviewViewModel view = await this.CreateItemsOverviewModel(category);

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> EditDough(int id)
        {
            EditDoughInputModel model = await this._menuManagementService.GetDoughDetailsByIdAsync(id);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Sauces()
        {
            ManagementCategory category = ManagementCategory.Sauce;

            AdminItemsOverviewViewModel view = await this.CreateItemsOverviewModel(category);
            

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> Toppings()
        {
            ManagementCategory category = ManagementCategory.Topping;

            AdminItemsOverviewViewModel view = await this.CreateItemsOverviewModel(category);

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> ToppingCategories()
        {
            ManagementCategory category = ManagementCategory.ToppingCategory;

            AdminItemsOverviewViewModel view = await this.CreateItemsOverviewModel(category);

            return this.View("Items", view);
        }

        private async Task<AdminItemsOverviewViewModel> CreateItemsOverviewModel(ManagementCategory category)
        {
            IEnumerable<MenuItemViewModel> toppings = await this._menuManagementService
                            .GetAllItemsFromCategory(category);

            AdminItemsOverviewViewModel view = new()
            {
                Category = category,
                Items = toppings
            };
            return view;
        }

        [HttpGet]
        public async Task<IActionResult> EditPizza(int id)
        {
            EditAdminPizzaInputModel? pizza = await this._menuManagementService.GetPizzaDetailsByIdAsync(id);

            if (pizza is null)
            {
                return this.NotFound();
            }

            return this.View(pizza);
        }

        [HttpPost]
        public async Task<IActionResult> EditPizza(AdminPizzaInputModel pizza)
        {
            await this._menuManagementService.EditPizzaAsync(pizza);
            return this.RedirectToAction(nameof(EditPizza), new { id =  pizza.Id });
        }
    }
}
