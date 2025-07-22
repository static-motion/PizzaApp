namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Pizzas;

    public class PizzasController : BaseController
    {
        private readonly IPizzaService _pizzaService;
        public PizzasController(IPizzaService pizzaService)
        {
            this._pizzaService = pizzaService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(CreatePizzaViewModel createPizza)
        {

            createPizza = new()
            {
                Ingredients = await this._pizzaService.GetAllIngredientsAsync()
            };

            return this.View(createPizza);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreatePizzaViewModel createdPizza)
        {
            if (!this.ModelState.IsValid || createdPizza.Pizza is null)
                return this.RedirectToAction(nameof(Create), createdPizza);

            string userId = this.GetUserId()!; // BaseController has the global [Authorize] attribute
                                               // userId should not be null

            await this._pizzaService.CreatePizzaAsync(createdPizza.Pizza, 
                createdPizza.SelectedToppingIds, 
                userId);
            return this.RedirectToAction(nameof(Create));
        }
    }
}
