namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.GCommon.Extensions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.ShoppingCart;

    public class CartController :  BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            this._cartService = cartService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(this.GetUserId()!);

            ShoppingCartItemsViewModel shoppingCart =
                await this._cartService.GetUserCart(userId);

            return this.View("AltIndexView", shoppingCart);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem([FromForm] int itemId, [FromForm] string category)
        {
            MenuCategory menuCategory = MenuCategoryExtensions.FromString(category)
                ?? throw new ArgumentException("Invalid category");

            string userId = this.GetUserId()!;

            bool isRemoved = await this._cartService.RemoveItemFromCartAsync(itemId, userId, menuCategory);
            return this.RedirectToAction(nameof(Index));
        }
    }
}
