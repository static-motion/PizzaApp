namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class PizzasController : BaseAdminController
    {
        private readonly IPizzaManagementService _pizzaManagementService;
        private readonly IIngredientsService _ingredientsService;
        public PizzasController(IPizzaManagementService pizzaManagementService,               
            IIngredientsService ingredientsService)
        {
            this._pizzaManagementService = pizzaManagementService;
            this._ingredientsService = ingredientsService;
        }

        public IActionResult Index()
        {
            return this.RedirectToAction("Pizzas", "Overview");
        }

        [HttpGet]
        public async Task<IActionResult> EditPizza(int id)
        {
            try
            {
                Guid userId = this.GetUserId()!.Value;
                // create the model
                EditBasePizzaInputModel pizza
                    = await this._pizzaManagementService.GetBasePizzaAsync(id, userId);

                EditBasePizzaViewWrapper wrapper = new()

                {
                    Ingredients = await this._ingredientsService
                        .GetAllIngredientsAsync(ignoreFiltering: true, disaleTracking: true),
                    Pizza = pizza
                };

                return this.View(wrapper);
            }
            catch (Exception)
            {
                return this.NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditPizza(EditBasePizzaViewWrapper pizzaViewWrapper)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO:
                return this.BadRequest();
            }
            try
            {
                Guid userId = this.GetUserId()!.Value;
                EditBasePizzaInputModel pizza = pizzaViewWrapper.Pizza;
                await this._pizzaManagementService.EditPizzaAsync(pizza, userId);
                return this.RedirectToAction(nameof(EditPizza), new { id = pizza.Id });
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreatePizza()
        {
            try
            {
                CreateBasePizzaViewWrapper createPizza = new()
                {
                    Ingredients = await this._ingredientsService
                        .GetAllIngredientsAsync(ignoreFiltering: true, disaleTracking: true)
                };
                return this.View(createPizza);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePizza(CreateBasePizzaViewWrapper viewWrapper)
        {
            Guid? userId = this.GetUserId();

            CreateBasePizzaInputModel inputPizza = viewWrapper.Pizza;
            await this._pizzaManagementService.CreateBasePizzaAsync(inputPizza, userId!.Value);
            return this.RedirectToAction(nameof(Index));
        }
    }
}
