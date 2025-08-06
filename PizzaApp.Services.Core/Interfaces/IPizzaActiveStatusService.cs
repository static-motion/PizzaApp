namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Data.Models;

    public interface IPizzaActiveStatusService
    {
        public bool IsPizzaActive(Pizza pizza);
    }
}
