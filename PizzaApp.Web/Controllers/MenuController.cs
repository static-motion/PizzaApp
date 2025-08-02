namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PizzaApp.GCommon.Enums;
    using PizzaApp.GCommon.Extensions;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Services.Common.Exceptions;
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
            try
            {
                MenuCategory? categoryEnum = MenuCategoryExtensions.FromString(category);

                if (categoryEnum is null)
                    return this.NotFound();

                IEnumerable<MenuItemViewModel> menuItems
                    = await this._menuService.GetAllMenuItemsForCategoryAsync(categoryEnum.Value);

                MenuCategoryViewWrapper menuView = new()
                {
                    Items = menuItems,
                    AllCategories = CategoryNames
                };

                return this.View(menuView);
            }
            catch (MenuCategoryNotImplementedException)
            {
                return this.BadRequest(); //TODO: Log message
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Pizzas(int id)
        {
            try
            {
                PizzaDetailsViewWrapper orderPizzaViewModel
                    = await this._menuService.GetPizzaDetailsByIdAsync(id);

                return this.View("PizzaDetails", orderPizzaViewModel);
            }
            catch (ItemNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception e)
            {
                return this.BadRequest(); // TODO: Log
            }
        }

        [HttpGet]
        public async Task<IActionResult> Drinks(int id)
        {
            try
            {
                MenuItemDetailsViewModel orderItem
                    = await this._menuService.GetDrinkDetailsById(id);
                return this.View("ItemDetails", orderItem);
            }
            catch (ItemNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Desserts(int id)
        {
            try
            {
                MenuItemDetailsViewModel orderItem
                    = await this._menuService.GetDessertDetailsById(id);
                return this.View("ItemDetails", orderItem);
            }
            catch (ItemNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPizzaToCart(PizzaDetailsViewWrapper? orderPizzaViewModel)
        {
            if (orderPizzaViewModel is null)
            {
                // TODO: Handle better
                return this.BadRequest("Invalid pizza details.");
            }
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(); // TODO:
            }

            Guid? userId = this.GetUserId();

            CustomizePizzaInputModel input = orderPizzaViewModel.Pizza;
            
            PizzaCartDto pizzaDto = new()
            {
                PizzaId = input.Id,
                DoughId = input.DoughId,
                SauceId = input.SauceId,
                SelectedToppingsIds = input.SelectedToppingIds.ToArray(),
                Quantity = input.Quantity
            };
            bool addedToCart = await this._cartService
                .AddPizzaToCartAsync(pizzaDto, userId!.Value);

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(MenuItemDetailsViewModel? orderItem)
        {
            if (orderItem is null)
            {
                // TODO: Handle better
                return this.BadRequest("Invalid menu item.");
            }
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
                // TODO: Handle
            }
            Guid? userId = this.GetUserId(); // TODO: handle null userId

            bool addedToCart = await this._cartService
                .AddItemToCartAsync(orderItem, userId!.Value);

            return this.RedirectToAction(nameof(Index));
        }
    }
}