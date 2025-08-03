namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Admin;

    public interface IMenuManagementService
    {
        Task EditDoughAsync(EditDoughInputModel model);
        Task EditPizzaAsync(BasePizzaInputModel pizza);
        Task EditSauceAsync(EditSauceInputModel inputSauce);
        Task EditToppingAsync(EditToppingInputModel inputTopping);
        Task EditToppingCategoryAsync(EditToppingCategoryInputModel inputModel);
        public Task<AdminItemsOverviewViewWrapper> GetItemsFromCategory(ManagementCategory category, int page, int pageSize);
        Task<EditDoughInputModel> GetDoughDetailsByIdAsync(int id);
        Task<EditBasePizzaViewWrapper?> GetPizzaDetailsByIdAsync(int id);
        Task<EditSauceInputModel> GetSauceDetailsByIdAsync(int id);
        Task<EditToppingCategoryInputModel> GetToppingCategoryDetailsByIdAsync(int id);
        Task<EditToppingViewWrapper> GetToppingDetailsByIdAsync(int id);
    }
}
