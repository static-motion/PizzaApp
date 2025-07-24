namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Menu;

    public interface IMenuService
    {
        public Task<IEnumerable<MenuItemViewModel>> GetAllDessertsForMenuAsync();
        public Task<IEnumerable<MenuItemViewModel>> GetAllDrinksForMenuAsync();
        public Task<IEnumerable<MenuItemViewModel>> GetAllPizzasForMenuAsync();
        public Task<IReadOnlyCollection<MenuItemViewModel>> GetAllMenuItemsForCategoryAsync(MenuCategory category);
        public Task<OrderPizzaViewModel?> GetPizzaDetailsByIdAsync(int id);
        public Task<OrderItemViewModel?> GetOrderItemDetailsAsync(int id, MenuCategory? categoryEnum);
    }
}
