namespace PizzaApp.Web.ViewModels.Pizzas
{
    public class CreatePizzaViewWrapper
    {
        public PizzaIngredientsViewWrapper? Ingredients { get; set; }
        public HashSet<int> SelectedToppingIds { get; set; } = new();
        public PizzaInputModel? Pizza { get; set; }
    }
}
