namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Web.ViewModels.Menu;
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICartService
    {
        Task<bool> AddItemToCartAsync(OrderItemViewModel orderItem, string userId);
        Task<bool> AddPizzaToCartAsync(PizzaCartDto pizzaDto, string userId);
        Task<ShoppingCartItemsViewModel> GetUserCart(Guid userId);
        Task<bool> RemoveItemFromCartAsync(int itemId, string userId, MenuCategory menuCategory);
    }
}
