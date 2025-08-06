namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Services.Core.Interfaces;

    public class PizzaActiveStatusService : IPizzaActiveStatusService
    {
        public bool IsPizzaActive(Pizza pizza)
        {
            return pizza.IsDeleted == false // the pizza must be active
                    && (pizza.Sauce == null || pizza.Sauce.IsDeleted == false) // the sauce must be either not set or active
                    && pizza.Dough.IsDeleted == false // the dough must be active
                    && pizza.Toppings.All(t => t.Topping.IsDeleted == false // all toppings must be active
                    && t.Topping.ToppingCategory.IsDeleted == false); // all categories must be active
        }
    }
}
