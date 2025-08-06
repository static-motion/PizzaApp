namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.GCommon.Extensions;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Cart;

    public class CartController :  BaseController
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            this._cartService = cartService;
            this._orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                Guid userId = this.GetUserId()!.Value;

                CartViewWrapper shoppingCart =
                    await this._cartService.GetUserCart(userId);

                return this.View(shoppingCart);
            }
            catch (Exception ex)
            {
                // TODO: Log message
                return this.StatusCode(500);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem([FromForm] int itemId, [FromForm] string category)
        {
            try
            {
                MenuCategory menuCategory = MenuCategoryExtensions.FromString(category)
                ?? throw new ArgumentException("Invalid category");

                Guid? userId = this.GetUserId();

                await this._cartService.RemoveItemFromCartAsync(itemId, userId!.Value, menuCategory);
                return this.RedirectToAction(nameof(Index));
            } 
            catch (ArgumentException ex)
            {
                return this.BadRequest();
            }
            catch (EntityNotFoundException)
            {
                return this.BadRequest();
            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CartViewWrapper cart)
        {
            if (!this.ModelState.IsValid) // TODO: handle better lol
            {
                return this.BadRequest();
            }

            Guid? userId = this.GetUserId();
            await this._orderService.PlaceOrderAsync(cart.OrderDetails, userId!.Value);
            await this._cartService.ClearShoppingCart(userId!.Value);

            return this.RedirectToAction(nameof(Index));
        }
    }
}
