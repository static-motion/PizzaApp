namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Admin;

    public interface IMenuManagementService
    {
        Task EditDoughAsync(EditDoughInputModel model);
        Task EditPizzaAsync(AdminPizzaInputModel pizza);
        Task EditSauceAsync(EditSauceInputModel inputSauce);
        public Task<IEnumerable<MenuItemViewModel>> GetAllItemsFromCategory(ManagementCategory category);
        Task<EditDoughInputModel> GetDoughDetailsByIdAsync(int id);
        Task<EditAdminPizzaInputModel?> GetPizzaDetailsByIdAsync(int id);
        Task<EditSauceInputModel> GetSauceDetailsByIdAsync(int id);
    }
}
