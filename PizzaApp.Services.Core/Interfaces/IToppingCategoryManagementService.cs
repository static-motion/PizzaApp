namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Admin;

    public interface IToppingCategoryManagementService
    {
        Task<(int, IEnumerable<MenuItemViewModel>)> GetToppingCategoriesOverviewPaginatedAsync(int page, int pageSize);
        Task<EditToppingCategoryInputModel> GetToppingCategoryDetailsByIdAsync(int id);
        Task EditToppingCategoryAsync(EditToppingCategoryInputModel inputModel);
    }
}
