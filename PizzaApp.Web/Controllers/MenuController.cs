namespace PizzaApp.Web.Controllers
{
    using Humanizer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Data.Common.Exceptions;
    using PizzaApp.GCommon.Enums;
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

        private readonly ICartService _cartService;
        private readonly IPizzaMenuService _pizzaMenuService;
        private readonly IDessertMenuService _dessertMenuService;
        private readonly IDrinkMenuService _drinkMenuService;

        public MenuController(IPizzaMenuService pizzaMenuService, 
            ICartService cartService,
            IDrinkMenuService drinkMenuService,
            IDessertMenuService dessertMenuService)
        {
            this._cartService = cartService;
            this._pizzaMenuService = pizzaMenuService;
            this._dessertMenuService = dessertMenuService;
            this._drinkMenuService = drinkMenuService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.RedirectToAction(nameof(Pizzas));
        }

        [HttpGet]
        public async Task<IActionResult> Pizzas()
        {
            try
            {
                IEnumerable<MenuItemViewModel> menuItems
                    = await this._pizzaMenuService.GetAllBaseItemsAsync();

                MenuCategoryViewWrapper menuView = new()
                {
                    Items = menuItems,
                    AllCategories = FormattedCategoryNames,
                    Category = MenuCategory.Pizzas
                };

                return this.View("Category", menuView);
            }
            catch (Exception)
            {
                return this.BadRequest(); // TODO: Log
            }
        }

        [HttpGet("/Menu/Pizzas/{id}")]
        public async Task<IActionResult> Pizzas(int id)
        {
            try
            {
                PizzaDetailsViewWrapper orderPizzaViewModel
                    = await this._pizzaMenuService.GetPizzaDetailsByIdAsync(id);

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
        public async Task<IActionResult> MyPizzas()
        {
            try
            {
                Guid? userId = this.GetUserId();
                IEnumerable<MenuItemViewModel> menuItems
                    = await this._pizzaMenuService.GetAllUserPizzasAsync(userId!.Value);

                MenuCategoryViewWrapper menuView = new()
                {
                    Items = menuItems,
                    AllCategories = FormattedCategoryNames,
                    Category = MenuCategory.MyPizzas
                };

                return this.View("Category", menuView);
            }
            catch (Exception)
            {
                return this.BadRequest(); // TODO: Log
            }
        }

        [HttpGet("/Menu/MyPizzas/{id}")]
        public async Task<IActionResult> MyPizzas(int id)
        {
            try
            {
                PizzaDetailsViewWrapper orderPizzaViewModel
                    = await this._pizzaMenuService.GetPizzaDetailsByIdAsync(id);

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
        public async Task<IActionResult> Drinks()
        {
            try
            {
                IEnumerable<MenuItemViewModel> menuItems
                    = await this._drinkMenuService.GetAllBaseItemsAsync();

                MenuCategoryViewWrapper menuView = new()
                {
                    Items = menuItems,
                    AllCategories = FormattedCategoryNames,
                    Category = MenuCategory.Drinks
                };

                return this.View("Category", menuView);
            }
            catch (Exception)
            {
                return this.BadRequest(); // TODO: Log
            }
        }

        [HttpGet("/Menu/Drinks/{id}")]
        public async Task<IActionResult> Drinks(int id)
        {
            try
            {
                MenuItemDetailsViewModel orderItem
                    = await this._drinkMenuService.GetDetailsById(id);
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
        public async Task<IActionResult> Desserts()
        {
            try
            {
                IEnumerable<MenuItemViewModel> menuItems
                    = await this._dessertMenuService.GetAllBaseItemsAsync();

                MenuCategoryViewWrapper menuView = new()
                {
                    Items = menuItems,
                    AllCategories = FormattedCategoryNames,
                    Category = MenuCategory.Desserts
                };

                return this.View("Category", menuView);
            }
            catch (Exception)
            {
                return this.BadRequest(); // TODO: Log
            }
        }

        [HttpGet("/Menu/Desserts/{id}")]
        public async Task<IActionResult> Desserts(int id)
        {
            try
            {
                MenuItemDetailsViewModel orderItem
                    = await this._dessertMenuService.GetDetailsById(id);
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
            try
            {
                await this._cartService.AddPizzaToCartAsync(pizzaDto, userId!.Value);

                return this.RedirectToAction(nameof(Index));
            } 
            catch (EntityNotFoundException)
            {
                return this.BadRequest();
            }
            catch (EntityRangeCountMismatchException)
            {
                return this.BadRequest();
            }
            catch (Exception ex)
            {
                return this.StatusCode(500);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(MenuItemDetailsViewModel? orderItem)
        {
            if (orderItem is null || !this.ModelState.IsValid)
            {
                // TODO: Handle better
                return this.BadRequest("Invalid menu item.");
            }

            Guid? userId = this.GetUserId(); // TODO: handle null userId

            try
            {
                await this._cartService.AddItemToCartAsync(orderItem, userId!.Value);

                return this.RedirectToAction(nameof(Index));
            }
            catch (EntityNotFoundException)
            {
                return this.BadRequest();
            }
            catch (EntityRangeCountMismatchException)
            {
                return this.BadRequest();
            }
            catch (Exception ex)
            {
                return this.StatusCode(500);
            }
        }
    }
}