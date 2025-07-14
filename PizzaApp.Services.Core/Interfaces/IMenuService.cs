namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Menu;

    public interface IMenuService
    {
        Task<IEnumerable<MenuItemViewModel>> GetAllDessertsForMenuAsync();
        Task<IEnumerable<MenuItemViewModel>> GetAllDrinksForMenuAsync();
        Task<IEnumerable<MenuItemViewModel>> GetAllMenuItemsForCategoryAsync(MenuCategory category);
        public Task<IEnumerable<MenuItemViewModel>> GetAllPizzasForMenuAsync();
        Task<OrderPizzaViewModel?> GetPizzaDetailsByIdAsync(int id);
    }
}
