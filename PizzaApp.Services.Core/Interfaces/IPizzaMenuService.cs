namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Menu;
    using PizzaApp.Web.ViewModels.Pizzas;
    using System;
    using System.Collections.Generic;

    public interface IPizzaMenuService : IMenuCategoryService
    {
        Task<IEnumerable<MenuItemViewModel>> GetAllUserPizzasAsync(Guid value);
        Task<PizzaDetailsViewWrapper> GetPizzaDetailsByIdAsync(int id);
    }
}
