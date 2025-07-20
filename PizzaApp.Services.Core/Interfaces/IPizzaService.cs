namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels;
    using PizzaApp.Web.ViewModels.Pizzas;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPizzaService
    {
        Task<bool> CreatePizzaAsync(PizzaInputModel pizza, List<int> selectedToppingIds, string userId);
        Task<PizzaIngredientsViewModel> GetAllIngredientsAsync();
    }
}
