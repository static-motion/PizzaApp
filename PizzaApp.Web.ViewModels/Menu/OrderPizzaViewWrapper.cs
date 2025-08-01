namespace PizzaApp.Web.ViewModels.Menu
{
    public class OrderPizzaViewWrapper 
    {
        public PizzaIngredientsViewWrapper? Ingredients { get; set; } = null!;
        public HashSet<int> SelectedToppingIds { get; set; } = new();
        public PizzaDetailsViewModel Pizza { get; set; } = null!;
    }
}

