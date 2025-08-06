namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IToppingManagementService
    {
        Task<(int, IEnumerable<MenuItemViewModel>)> GetToppingsOverviewPaginatedAsync(int page, int pageSize);
        Task<EditToppingViewWrapper> GetToppingDetailsByIdAsync(int id);
        Task EditToppingAsync(EditToppingInputModel inputTopping);
    }
}
