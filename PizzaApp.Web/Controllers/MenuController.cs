namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;

    public class MenuController : Controller
    {
        private readonly IPizzaService _pizzaService;
        public MenuController(IPizzaService pizzaService)
        {
            this._pizzaService = pizzaService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<MenuPizzaViewModel> allPizzas 
                = await this._pizzaService.GetAllPizzasForMenuAsync();

            return this.View(allPizzas);
        }
    }
}
