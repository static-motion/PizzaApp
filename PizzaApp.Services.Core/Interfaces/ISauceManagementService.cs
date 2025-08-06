namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISauceManagementService
    {
        Task<(int, IEnumerable<MenuItemViewModel>)> GetSaucesOverviewPaginatedAsync(int page, int pageSize);
        Task<EditSauceInputModel> GetSauceDetailsByIdAsync(int id);
        Task EditSauceAsync(EditSauceInputModel inputSauce);
    }
}
