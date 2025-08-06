namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.CodeActions;
    using PizzaApp.Data.Common.Exceptions;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Interfaces;
    using PizzaApp.Web.ViewModels.MyPizzas;
    using PizzaApp.Web.ViewModels.Pizzas;

    public class MyPizzasController : BaseController
    {
        private readonly IPizzaCreationService _pizzaCreationService;
        private readonly IIngredientsService _ingredientsService;
        private readonly IPizzaEditingService _pizzaEditingService;
        public MyPizzasController(IPizzaCreationService pizzaCreationService, 
            IIngredientsService ingredientsService,
            IPizzaEditingService pizzaEditingService)
        {   
            this._pizzaCreationService = pizzaCreationService;
            this._ingredientsService = ingredientsService;
            this._pizzaEditingService = pizzaEditingService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.RedirectToAction("MyPizzas", "Menu");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                CreatePizzaViewWrapper createPizza = new()
                {
                    Ingredients = await this._ingredientsService.GetAllIngredientsAsync()
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
        public async Task<IActionResult> Create(CreatePizzaViewWrapper createdPizza)
        {
            if (!this.ModelState.IsValid || createdPizza.Pizza is null)
                return this.BadRequest();

            Guid? userId = this.GetUserId(); // BaseController has the global [Authorize] attribute
                                             // userId should not be null
            CreateCustomerPizzaInputModel pizzaInput = createdPizza.Pizza;
            try
            {
                await this._pizzaCreationService.CreatePizzaAsync(pizzaInput, userId!.Value);
                return this.RedirectToAction("MyPizzas", "Menu");
            }
            catch (EntityNotFoundException)
            {
                return this.BadRequest();
            }
            catch (EntityRangeCountMismatchException)
            {
                return this.BadRequest();
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized();
            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Guid? userId = this.GetUserId();
                EditCustomerPizzaViewWrapper wrapper = new()
                {
                    Ingredients = await this._ingredientsService.GetAllIngredientsAsync(),
                    Pizza = await this._pizzaEditingService.GetCustomerPizzaToEdit(id, userId!.Value)
                };
                return this.View(wrapper);
            }
            catch (EntityNotFoundException ex)
            {
                return this.BadRequest();
            }
            catch (UserNotOwnerException)
            {
                return this.Forbid();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerPizzaViewWrapper wrapper)
        {
            try
            {
                Guid? userId = this.GetUserId();
                IEditPizzaInputModel pizza = wrapper.Pizza;
                await this._pizzaEditingService.EditPizzaAsync(pizza, userId!.Value);
                return this.RedirectToAction("MyPizzas", "Menu");
            }
            catch (EntityNotFoundException ex)
            {
                return this.BadRequest();
            }
            catch (EntityRangeCountMismatchException)
            {
                return this.BadRequest();
            }
            catch (InvalidOperationException)
            {
                return this.Unauthorized();
            }
            catch (UserNotOwnerException)
            {
                return this.Forbid();
            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
        }
    }
}
