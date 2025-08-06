namespace PizzaApp.Web.ViewModels.Admin
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System.ComponentModel.DataAnnotations;

    public class CreateBasePizzaViewWrapper
    {
        [ValidateNever]
        public PizzaIngredientsViewWrapper Ingredients { get; set; } = null!;

        [Required]
        public CreateBasePizzaInputModel Pizza { get; set; } = null!;
    }
}
