namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Menu;

    public interface IMenuService
    {
        public Task<IReadOnlyCollection<MenuItemViewModel>> GetAllMenuItemsForCategoryAsync(MenuCategory category);
        public Task<PizzaDetailsViewWrapper> GetPizzaDetailsByIdAsync(int id);
        Task<MenuItemDetailsViewModel> GetDrinkDetailsById(int id);
        Task<MenuItemDetailsViewModel> GetDessertDetailsById(int id);
    }
}
