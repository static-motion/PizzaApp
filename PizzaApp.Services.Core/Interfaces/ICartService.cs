namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Web.ViewModels.Menu;
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using System;
    using System.Threading.Tasks;

    public interface ICartService
    {
        Task<bool> AddItemToCartAsync(MenuItemDetailsViewModel orderItem, Guid userId);
        Task AddPizzaToCartAsync(PizzaCartDto pizzaDto, Guid userId);
        Task<CartViewWrapper> GetUserCart(Guid userId);
        Task<bool> RemoveItemFromCartAsync(int itemId, Guid userId, MenuCategory menuCategory);
        Task ClearShoppingCart(Guid userId);
    }
}
