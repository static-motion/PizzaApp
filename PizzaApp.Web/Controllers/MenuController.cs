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
            MenuCategory? categoryEnum = MenuCategoryExtensions.FromString(category);

            if (categoryEnum is null)
                return this.NotFound();

            IEnumerable<MenuItemViewModel> menuItems 
                = await this._menuService.GetAllMenuItemsForCategoryAsync(categoryEnum.Value);

            MenuCategoryViewModel menuView = new()
            {
                Items = menuItems,
                AllCategories = CategoryNames
            };

            return this.View(menuView);
        }

        [HttpGet("/Menu/{category}/{id:int}")]
        public async Task<IActionResult> ItemDetails(int id, string category)
        {
            MenuCategory? categoryEnum = MenuCategoryExtensions.FromString(category);

            if (categoryEnum is null)
                return this.NotFound();

            if (categoryEnum == MenuCategory.Pizzas)
            {
                OrderPizzaViewModel? orderPizzaViewModel 
                    = await this._menuService.GetPizzaDetailsByIdAsync(id);

                if (orderPizzaViewModel is null)
                    return this.NotFound();

                return this.View("PizzaDetails", orderPizzaViewModel);
            }

            OrderItemViewModel? orderItem 
                = await this._menuService.GetOrderItemDetailsAsync(id, categoryEnum);

            if (orderItem is null)
                return this.NotFound();

            return this.View("ItemDetails", orderItem);
        }
        [HttpPost]
        public async Task<IActionResult> AddPizzaToCart(OrderPizzaViewModel? orderPizzaViewModel)
        {
            if (orderPizzaViewModel is null)
            {
                // TODO: Handle better
                return this.BadRequest("Invalid pizza details.");
            }
            Guid? userId = this.GetUserId();

            PizzaCartDto pizzaDto = new()
            {
                PizzaId = orderPizzaViewModel.Pizza.Id,
                DoughId = orderPizzaViewModel.Pizza.DoughId,
                SauceId = orderPizzaViewModel.Pizza.SauceId,
                SelectedToppingsIds = orderPizzaViewModel.SelectedToppingIds,
                Quantity = orderPizzaViewModel.Pizza.Quantity
            };
            bool addedToCart = await this._cartService.AddPizzaToCartAsync(pizzaDto, userId!.Value);
            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(OrderItemViewModel? orderItem)
        {
            if (orderItem is null)
            {
                // TODO: Handle better
                return this.BadRequest("Invalid menu item.");
            }
            if (!this.ModelState.IsValid)
            {
                // TODO: Handle
            }
            Guid? userId = this.GetUserId(); // TODO: handle null userId

            bool addedToCart = await this._cartService.AddItemToCartAsync(orderItem, userId!.Value);
            return this.RedirectToAction(nameof(Index));
        }
    }
}