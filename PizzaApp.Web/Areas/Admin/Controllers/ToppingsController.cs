namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class ToppingsController : BaseAdminController
    {
        private readonly IToppingManagementService _toppingManagementService;


        public ToppingsController(IToppingManagementService toppingManagementService)
        {
            this._toppingManagementService = toppingManagementService;
        }

        public IActionResult Index()
        {
            return this.RedirectToAction("Toppings", "Overview");
        }

        [HttpGet]
        public async Task<IActionResult> EditTopping(int id)
        {
            EditToppingViewWrapper toppingView
                = await this._toppingManagementService
                            .GetToppingDetailsByIdAsync(id);

            return this.View(toppingView);
        }

        [HttpPost]
        public async Task<IActionResult> EditTopping(EditToppingViewWrapper toppingViewWrapper)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO:
            }

            EditToppingInputModel toppingInputModel = toppingViewWrapper.ToppingInputModel;


            await this._toppingManagementService.EditToppingAsync(toppingInputModel);

            return this.RedirectToAction(nameof(EditTopping), new { id = toppingInputModel.Id });

        }
    }
}
