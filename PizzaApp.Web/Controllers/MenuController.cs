namespace PizzaApp.Web.Controllers
{
    using Humanizer;
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
        private readonly static Dictionary<MenuCategory, string> FormattedCategoryNames 
            = Enum.GetValues<MenuCategory>()
                  .ToDictionary(mc => mc, mc => mc.ToString()
                                                  .Humanize(LetterCasing.Title));

        private readonly IMenuService _menuService;
        private readonly ICartService _cartService;

        public MenuController(IMenuService menuService, ICartService cartService)
        {
            this._menuService = menuService;
            this._cartService = cartService;
        }

        [HttpGet]
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
                    AllCategories = FormattedCategoryNames,
                    Category = categoryEnum.Value
                };

                return this.View(menuView);
            }
            catch (MenuCategoryNotImplementedException)
            {
                return this.NotFound(); //TODO: Log message
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
            catch (EntityNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
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
            catch (EntityNotFoundException)
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
            catch (EntityNotFoundException)
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
                return this.BadRequest();
            }
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
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

            await this._cartService.AddPizzaToCartAsync(pizzaDto, userId!.Value);

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