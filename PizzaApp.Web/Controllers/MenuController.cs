namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using PizzaApp.GCommon.Enums;
    using PizzaApp.GCommon.Extensions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;

    public class MenuController : Controller
    {
        private static IEnumerable<string> CategoryNames = Enum.GetNames<MenuCategory>();

        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            this._menuService = menuService;
        }

        [HttpGet]
        [Route("/Menu/Index")]
        [Route("/Menu")]
        public IActionResult Index()
        {
            return this.RedirectToAction(nameof(Category), new { category = MenuCategory.Pizzas });
        }

        [HttpGet("/Menu/{category}")]
        public async Task<IActionResult> Category(string category)
        {
            MenuCategory? categoryEnum = MenuCategoryExtensions.FromUrlString(category);

            if (categoryEnum is null)
                return this.NotFound();

            IEnumerable<MenuItemViewModel> menuItems;

            switch (categoryEnum)
            {
                case MenuCategory.Pizzas:
                    menuItems = await this._menuService.GetAllPizzasForMenuAsync();
                    break;
                case MenuCategory.Drinks:
                    menuItems = await this._menuService.GetAllDrinksForMenuAsync();
                    break;
                case MenuCategory.Desserts:
                    menuItems = await this._menuService.GetAllDessertsForMenuAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unsupported menu category: {categoryEnum}");
            }
            MenuCategoryViewModel menuView = new()
            {
                Category = categoryEnum.Value,
                Items = menuItems,
                AllCategories = CategoryNames
            };
            return this.View(menuView);
        }
    }
}