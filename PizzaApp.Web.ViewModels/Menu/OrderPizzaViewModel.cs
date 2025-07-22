namespace PizzaApp.Web.ViewModels.Menu
{
    public class OrderPizzaViewModel 
    {
        public PizzaIngredientsViewModel? Ingredients { get; set; } = null!;
        public HashSet<int> SelectedToppingIds { get; set; } = new();
        public PizzaDetailsViewModel Pizza { get; set; } = null!;
    }
}

