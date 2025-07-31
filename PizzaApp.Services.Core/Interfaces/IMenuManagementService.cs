namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Admin;

    public interface IMenuManagementService
    {
        Task EditPizzaAsync(AdminPizzaInputModel pizza);
        public Task<IEnumerable<MenuItemViewModel>> GetAllItemsFromCategory(ManagementCategory category);
        Task<EditDoughInputModel> GetDoughDetailsByIdAsync(int id);
        Task<EditAdminPizzaInputModel?> GetPizzaDetailsByIdAsync(int id);
    }
}
