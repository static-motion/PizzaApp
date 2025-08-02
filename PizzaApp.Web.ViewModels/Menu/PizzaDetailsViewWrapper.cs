namespace PizzaApp.Web.ViewModels.Menu
{
    public class PizzaDetailsViewWrapper 
    {
        public PizzaIngredientsViewWrapper? Ingredients { get; set; } = null!;
        public CustomizePizzaInputModel Pizza { get; set; } = null!;
    }
}

