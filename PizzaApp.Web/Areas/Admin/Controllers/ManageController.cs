﻿namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class ManageController : BaseAdminController
    {
        private const int PageSize = 10;
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
        public async Task<IActionResult> Pizzas(int page = 1)
        {
            ManagementCategory category = ManagementCategory.Pizza;

            AdminItemsOverviewViewWrapper view 
                = await this._menuManagementService.GetItemsFromCategory(category, page, PageSize);

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> Doughs(int page = 1)
        {
            ManagementCategory category = ManagementCategory.Dough;

            AdminItemsOverviewViewWrapper view
                = await this._menuManagementService.GetItemsFromCategory(category, page, PageSize);

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
        public async Task<IActionResult> Sauces(int page = 1)
        {
            ManagementCategory category = ManagementCategory.Sauce;

            AdminItemsOverviewViewWrapper view
                = await this._menuManagementService.GetItemsFromCategory(category, page, PageSize);


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
        public async Task<IActionResult> Toppings(int page = 1)
        {
            ManagementCategory category = ManagementCategory.Topping;

            AdminItemsOverviewViewWrapper view 
                = await this._menuManagementService.GetItemsFromCategory(category, page, PageSize);

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> EditTopping(int id)
        {
            EditToppingViewWrapper toppingView 
                = await this._menuManagementService
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

            //return this.RedirectToAction(nameof(EditTopping), new { id = toppingInputModel.Id });

            await this._menuManagementService.EditToppingAsync(toppingInputModel);

            return this.RedirectToAction(nameof(EditTopping), new { id = toppingInputModel.Id });

        }

        [HttpGet]
        public async Task<IActionResult> ToppingCategories(int page = 1)
        {
            ManagementCategory category = ManagementCategory.ToppingCategory;

            AdminItemsOverviewViewWrapper view
                = await this._menuManagementService.GetItemsFromCategory(category, page, PageSize);

            return this.View("Items", view);
        }

        [HttpGet]
        public async Task<IActionResult> EditToppingCategory(int id)
        {
            EditToppingCategoryInputModel model
                = await this._menuManagementService.GetToppingCategoryDetailsByIdAsync(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditToppingCategory(EditToppingCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                //TODO:
            }

            await this._menuManagementService.EditToppingCategoryAsync(inputModel);
            return this.RedirectToAction(nameof(EditToppingCategory), new { id = inputModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> EditPizza(int id)
        {
            EditAdminPizzaViewWrapper? pizza = await this._menuManagementService.GetPizzaDetailsByIdAsync(id);

            if (pizza is null)
            {
                return this.NotFound();
            }

            return this.View(pizza);
        }

        [HttpPost]
        public async Task<IActionResult> EditPizza(EditAdminPizzaViewWrapper pizzaViewWrapper)
        {
            // TODO:
            AdminPizzaInputModel pizza = pizzaViewWrapper.Pizza;
            await this._menuManagementService.EditPizzaAsync(pizza);
            return this.RedirectToAction(nameof(EditPizza), new { id =  pizza.Id });
        }
    }
}
