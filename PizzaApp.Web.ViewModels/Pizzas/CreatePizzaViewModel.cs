namespace PizzaApp.Web.ViewModels.Pizzas
{
    public class CreatePizzaViewModel
    {
        public PizzaIngredientsViewModel? Ingredients { get; set; } = null!;
        public HashSet<int> SelectedToppingIds { get; set; } = new();
        public PizzaInputModel? Pizza { get; set; }
    }
}
