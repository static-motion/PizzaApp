namespace PizzaApp.Web.ViewModels.Menu
{
    public class ToppingCategoryViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public IReadOnlyList<ToppingViewModel> Toppings { get; set; } = null!;
    }
}
