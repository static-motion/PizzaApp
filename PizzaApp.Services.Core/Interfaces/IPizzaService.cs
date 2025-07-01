namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels;

    public interface IPizzaService
    {
        public Task<IEnumerable<MenuPizzaViewModel>> GetAllPizzasForMenuAsync(); 
    }
}
