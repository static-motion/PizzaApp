namespace PizzaApp.Web.ViewModels.Menu
{
    public class ToppingCategoryViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public IEnumerable<ToppingViewModel> Toppings { get; set; } = null!;
    }
}
