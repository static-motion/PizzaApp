namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Admin;

    public interface IMenuManagementService
    {
        public Task<IEnumerable<ItemViewModel>> GetAllItemsFromCategory(ManagementCategory category);
    }
}
