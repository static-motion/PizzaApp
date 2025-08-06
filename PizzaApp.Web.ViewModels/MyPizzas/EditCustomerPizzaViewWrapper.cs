namespace PizzaApp.Web.ViewModels.MyPizzas
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class EditCustomerPizzaViewWrapper
    {
        [ValidateNever]
        public PizzaIngredientsViewWrapper Ingredients { get; set; } = null!;

        public EditCustomerPizzaInputModel Pizza { get; set; } = null!;
    }
}
