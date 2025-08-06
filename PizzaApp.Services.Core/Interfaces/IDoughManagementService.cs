namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDoughManagementService
    {
        Task<(int, IEnumerable<MenuItemViewModel>)> GetDoughsOverviewPaginatedAsync(int page, int pageSize);
        Task<EditDoughInputModel> GetDoughDetailsByIdAsync(int id);
        Task EditDoughAsync(EditDoughInputModel model);
    }

}
