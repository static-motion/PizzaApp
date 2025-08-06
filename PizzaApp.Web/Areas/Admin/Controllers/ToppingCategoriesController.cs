namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class ToppingCategoriesController : BaseAdminController
    {
        private readonly IToppingCategoryManagementService _toppingCategoryManagementService;


        public ToppingCategoriesController(IToppingCategoryManagementService toppingCategoryManagementService)
        {
            this._toppingCategoryManagementService = toppingCategoryManagementService;
        }

        public IActionResult Index()
        {
            return this.RedirectToAction("ToppingCategories", "Overview");
        }

        [HttpGet]
        public async Task<IActionResult> EditToppingCategory(int id)
        {
            EditToppingCategoryInputModel model
                = await this._toppingCategoryManagementService.GetToppingCategoryDetailsByIdAsync(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditToppingCategory(EditToppingCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                //TODO:
            }

            await this._toppingCategoryManagementService.EditToppingCategoryAsync(inputModel);
            return this.RedirectToAction(nameof(EditToppingCategory), new { id = inputModel.Id });
        }
    }
}
