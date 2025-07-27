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
            Guid? userId = this.GetUserId();

            ShoppingCartViewModel shoppingCart =
                await this._cartService.GetUserCart(userId!.Value);

            return this.View("IndexAlt", shoppingCart);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem([FromForm] int itemId, [FromForm] string category)
        {
            MenuCategory menuCategory = MenuCategoryExtensions.FromString(category)
                ?? throw new ArgumentException("Invalid category");

            Guid? userId = this.GetUserId();

            bool isRemoved = await this._cartService.RemoveItemFromCartAsync(itemId, userId!.Value, menuCategory);
            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderDetailsInputModel orderDetails)
        {
            if (!this.ModelState.IsValid) // TODO: handle better lol
            {
                return this.BadRequest(this.ModelState);
            }

            Guid? userId = this.GetUserId();
            throw new NotImplementedException();
        }
    }
}
