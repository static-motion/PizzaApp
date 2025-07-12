namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels;

    public interface IMenuService
    {
        Task<IEnumerable<MenuItemViewModel>> GetAllDessertsForMenuAsync();
        Task<IEnumerable<MenuItemViewModel>> GetAllDrinksForMenuAsync();
        public Task<IEnumerable<MenuItemViewModel>> GetAllPizzasForMenuAsync(); 
    }
}
