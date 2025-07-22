namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PizzaApp.GCommon.Enums;
    using PizzaApp.GCommon.Extensions;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Menu;

    public class MenuController : BaseController
    {
        private static IEnumerable<string> CategoryNames = Enum.GetNames<MenuCategory>();

        private readonly IMenuService _menuService;
        private readonly ICartService _cartService;

        public MenuController(IMenuService menuService, ICartService cartService)
        {
            this._menuService = menuService;
            this._cartService = cartService;
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

        [HttpPost]
        public async Task<IActionResult> AddToCart(OrderPizzaViewModel? orderPizzaViewModel)
        {
            if (orderPizzaViewModel is null)
            {
                // TODO: Handle better
                return this.BadRequest("Invalid pizza details.");
            }
            string userId = this.GetUserId()!;

            PizzaCartDto pizzaDto = new()
            {
                PizzaId = orderPizzaViewModel.Pizza.Id,
                DoughId = orderPizzaViewModel.Pizza.DoughId,
                SauceId = orderPizzaViewModel.Pizza.SauceId,
                SelectedToppingsIds = orderPizzaViewModel.SelectedToppingIds
            };
            bool addedToCart = await this._cartService.AddPizzaToCartAsync(pizzaDto, userId);
            return this.RedirectToAction(nameof(PizzaDetails), new { id = pizzaDto.PizzaId });
        }
    }
}