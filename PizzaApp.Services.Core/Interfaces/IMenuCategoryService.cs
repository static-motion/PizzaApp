namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Menu;

    public interface IMenuCategoryService
    {
        Task<IEnumerable<MenuItemViewModel>> GetAllBaseItemsAsync();
    }
}
