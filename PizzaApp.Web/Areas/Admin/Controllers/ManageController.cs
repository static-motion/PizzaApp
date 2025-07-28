namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class ManageController : BaseAdminController
    {
        private readonly IMenuManagementService _manageService;

        public ManageController(IMenuManagementService manageService)
        {
            this._manageService = manageService;
        }
        public IActionResult Index()
        {
            return this.RedirectToAction(nameof(Pizzas));
        }

        public async Task<IActionResult> Pizzas()
        {
            IEnumerable<ItemViewModel> pizzas = await this._manageService
                .GetAllItemsFromCategory(ManagementCategory.Pizzas);

            AdminItemsOverviewViewModel view = new()
            {
                Category = ManagementCategory.Pizzas,
                Items = pizzas
            };

            return this.View("Items", view);
        }
    }
}
