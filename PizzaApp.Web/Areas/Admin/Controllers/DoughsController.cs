namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class DoughsController : BaseAdminController
    {
        private readonly IDoughManagementService _doughManagementService;

        public DoughsController(IDoughManagementService doughManagementService)
        {
            this._doughManagementService = doughManagementService;
        }

        public IActionResult Index()
        {
            return this.RedirectToAction("Doughs", "Overview");
        }

        [HttpGet]
        public async Task<IActionResult> EditDough(int id)
        {
            EditDoughInputModel model = await this._doughManagementService
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

            await this._doughManagementService.EditDoughAsync(inputDough);

            return this.RedirectToAction(nameof(EditDough), new { id = inputDough.Id });
        }
    }
}
