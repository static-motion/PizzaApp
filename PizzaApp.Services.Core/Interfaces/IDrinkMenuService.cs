namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Menu;
    using System.Threading.Tasks;

    public interface IDrinkMenuService : IMenuCategoryService
    {
        Task<MenuItemDetailsViewModel> GetDetailsById(int id);
    }
}
