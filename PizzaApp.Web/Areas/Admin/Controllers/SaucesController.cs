namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class SaucesController : BaseAdminController
    {

        private readonly ISauceManagementService _sauceManagementService;

        public SaucesController(ISauceManagementService sauceManagementService)
        {
            this._sauceManagementService = sauceManagementService;
        }

        public IActionResult Index()
        {
            return this.RedirectToAction("Sauces", "Overview");
        }

        [HttpGet]
        public async Task<IActionResult> EditSauce(int id)
        {

            EditSauceInputModel sauce = await this._sauceManagementService.GetSauceDetailsByIdAsync(id);

            return this.View(sauce);
        }

        [HttpPost]
        public async Task<IActionResult> EditSauce(EditSauceInputModel inputSauce)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO:
            }

            await this._sauceManagementService.EditSauceAsync(inputSauce);

            return this.RedirectToAction(nameof(EditSauce), new { id = inputSauce.Id });
        }
    }
}
