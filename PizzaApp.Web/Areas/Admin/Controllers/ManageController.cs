namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class ManageController : BaseAdminController
    {
        private readonly IMenuManagementService _menuManagementService;

        public ManageController(IMenuManagementService menuManagementService)
        {
            this._menuManagementService = menuManagementService;
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
            EditDoughInputModel model = await this._menuManagementService
                .GetDoughDetailsByIdAsync(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDough(EditDoughInputModel inputDough)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO:
            }

            await this._menuManagementService.EditDoughAsync(inputDough);

            return this.RedirectToAction(nameof(EditDough), new { id = inputDough.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Sauces()
        {
            ManagementCategory category = ManagementCategory.Sauce;

            AdminItemsOverviewViewModel view = await this.CreateItemsOverviewModel(category);
            

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> EditSauce(int id)
        {

            EditSauceInputModel sauce = await this._menuManagementService.GetSauceDetailsByIdAsync(id);

            return this.View(sauce);
        }

        [HttpPost]
        public async Task<IActionResult> EditSauce(EditSauceInputModel inputSauce)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO:
            }

            await this._menuManagementService.EditSauceAsync(inputSauce);

            return this.RedirectToAction(nameof(EditSauce), new { id = inputSauce.Id });
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
            // TODO:
            await this._menuManagementService.EditPizzaAsync(pizza);
            return this.RedirectToAction(nameof(EditPizza), new { id =  pizza.Id });
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
    }
}
