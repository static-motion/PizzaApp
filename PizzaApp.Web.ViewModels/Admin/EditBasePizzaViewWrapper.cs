namespace PizzaApp.Web.ViewModels.Admin
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class EditBasePizzaViewWrapper
    {
        [ValidateNever]
        public PizzaIngredientsViewWrapper Ingredients { get; set; } = null!;
        public EditBasePizzaInputModel Pizza { get; set; } = null!;
    }
}
