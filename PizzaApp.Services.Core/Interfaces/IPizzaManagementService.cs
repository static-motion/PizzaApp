namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Admin;
    using System;

    public interface IPizzaManagementService
    {
        Task<(int, IEnumerable<MenuItemViewModel>)> GetPizzasOverviewPaginatedAsync(int page, int pageSize);
        Task<EditBasePizzaInputModel> GetBasePizzaAsync(int id, Guid userId);
        Task EditPizzaAsync(EditBasePizzaInputModel inputModel, Guid userId);
        Task CreateBasePizzaAsync(CreateBasePizzaInputModel inputPizza, Guid userId);
    }
}
