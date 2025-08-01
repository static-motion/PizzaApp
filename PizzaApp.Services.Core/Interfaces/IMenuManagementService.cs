namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Admin;

    public interface IMenuManagementService
    {
        Task EditDoughAsync(EditDoughInputModel model);
        Task EditPizzaAsync(AdminPizzaInputModel pizza);
        Task EditSauceAsync(EditSauceInputModel inputSauce);
        Task EditToppingAsync(EditToppingInputModel inputTopping);
        public Task<IEnumerable<MenuItemViewModel>> GetAllItemsFromCategory(ManagementCategory category);
        Task<EditDoughInputModel> GetDoughDetailsByIdAsync(int id);
        Task<EditAdminPizzaViewWrapper?> GetPizzaDetailsByIdAsync(int id);
        Task<EditSauceInputModel> GetSauceDetailsByIdAsync(int id);
        Task<EditToppingViewWrapper> GetToppingDetailsByIdAsync(int id);
    }
}
