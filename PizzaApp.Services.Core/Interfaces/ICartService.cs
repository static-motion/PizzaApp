namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Web.ViewModels.Cart;
    using PizzaApp.Web.ViewModels.Menu;
    using System;
    using System.Threading.Tasks;

    public interface ICartService
    {
        Task AddItemToCartAsync(MenuItemDetailsViewModel orderItem, Guid userId);
        Task AddPizzaToCartAsync(PizzaCartDto pizzaDto, Guid userId);
        Task<CartViewWrapper> GetUserCart(Guid userId);
        Task RemoveItemFromCartAsync(int itemId, Guid userId, MenuCategory menuCategory);
        Task ClearShoppingCart(Guid userId);
    }
}
