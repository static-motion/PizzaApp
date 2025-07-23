namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.ShoppingCart;

    public class CartController :  BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            this._cartService = cartService;
        }
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(this.GetUserId()!);

            IEnumerable<PizzaShoppingCartViewModel> pizzasFromCart =
                await this._cartService.GetUserCart(userId);

            return this.View(pizzasFromCart);
        }
    }
}
