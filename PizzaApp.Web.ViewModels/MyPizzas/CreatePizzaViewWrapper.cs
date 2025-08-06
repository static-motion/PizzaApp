namespace PizzaApp.Web.ViewModels.Pizzas
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class CreatePizzaViewWrapper
    {
        [ValidateNever]
        public PizzaIngredientsViewWrapper Ingredients { get; set; } = null!;

        public CreateCustomerPizzaInputModel Pizza { get; set; } = null!;
    }
}
