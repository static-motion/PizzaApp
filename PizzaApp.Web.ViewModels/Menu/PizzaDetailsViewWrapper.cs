namespace PizzaApp.Web.ViewModels.Menu
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class PizzaDetailsViewWrapper 
    {
        [ValidateNever]
        public PizzaIngredientsViewWrapper Ingredients { get; set; } = null!;
        public CustomizePizzaInputModel Pizza { get; set; } = null!;
    }
}

