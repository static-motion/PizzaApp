namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Admin;
    using PizzaApp.Web.ViewModels.Interfaces;
    using PizzaApp.Web.ViewModels.MyPizzas;
    using System;
    using System.Threading.Tasks;

    public interface IPizzaEditingService
    {
        Task EditPizzaAsync(IEditPizzaInputModel inputPizza, Guid userId);

        Task<EditBasePizzaInputModel> GetBasePizzaToEdit(int id, Guid userId);

        Task<EditCustomerPizzaInputModel> GetCustomerPizzaToEdit(int id, Guid userId);
    }
}
