namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Data.Models;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPizzaCartService
    {
        Task AddPizzaToCartAsync(PizzaCartDto pizzaDto, Guid userId);
        Task<IEnumerable<CartPizzaViewModel>> GetPizzasInCart(User user);
    }

}
