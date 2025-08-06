namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Interfaces;
    using System.Threading.Tasks;

    public interface IPizzaCreationService
    {
        Task CreatePizzaAsync(ICreatePizzaInputModel inputPizza, Guid userId);
    }
}
