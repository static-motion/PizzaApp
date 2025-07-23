namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICartService
    {
        Task<bool> AddPizzaToCartAsync(PizzaCartDto pizzaDto, string userId);
        Task<IEnumerable<PizzaShoppingCartViewModel>> GetUserCart(Guid userId);
    }
}
