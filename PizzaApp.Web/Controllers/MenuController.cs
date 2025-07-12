namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;

    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService pizzaService)
        {
            this._menuService = pizzaService;
        }

        [Route("/Menu")]
        [Route("/Menu/{category}")]
        public async Task<IActionResult> Index(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                category = "pizzas"; // Default category
            }

            IEnumerable<MenuItemViewModel> allItemsOfCategory = category.ToLower() switch
            {
                "pizzas" => await this._menuService.GetAllPizzasForMenuAsync(),
                "drinks" => await this._menuService.GetAllDrinksForMenuAsync(),
                "desserts" => await this._menuService.GetAllDessertsForMenuAsync(),
                _ => await this._menuService.GetAllPizzasForMenuAsync()
            };

            return this.View(allItemsOfCategory);
        }
    }
}
