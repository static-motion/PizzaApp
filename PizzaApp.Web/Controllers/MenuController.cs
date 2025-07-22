namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PizzaApp.GCommon.Enums;
    using PizzaApp.GCommon.Extensions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Menu;

    public class MenuController : BaseController
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
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.RedirectToAction(nameof(Category), new { category = MenuCategory.Pizzas });
        }

        [HttpGet("/Menu/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> Category(string category)
        {
            MenuCategory? categoryEnum = MenuCategoryExtensions.FromUrlString(category);

            if (categoryEnum is null)
                return this.NotFound();

            IEnumerable<MenuItemViewModel> menuItems 
                = await this._menuService.GetAllMenuItemsForCategoryAsync(categoryEnum.Value);
            

            MenuCategoryViewModel menuView = new()
            {
                Category = categoryEnum.Value, // not necessary for now, might clean up later TODO
                Items = menuItems,
                AllCategories = CategoryNames
            };

            return this.View(menuView);
        }

        [HttpGet("/Menu/Pizzas/{id:int}")]
        public async Task<IActionResult> PizzaDetails(int id)
        {
            OrderPizzaViewModel? orderPizzaViewModel = await this._menuService.GetPizzaDetailsByIdAsync(id);
            
            if (orderPizzaViewModel is null)
                return this.NotFound();

            return this.View(orderPizzaViewModel);
        }


    }
}