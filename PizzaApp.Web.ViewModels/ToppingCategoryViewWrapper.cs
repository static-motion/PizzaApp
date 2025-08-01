namespace PizzaApp.Web.ViewModels
{
    public class ToppingCategoryViewWrapper
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool IsActive { get; set; }

        public IReadOnlyList<ToppingViewModel> Toppings { get; set; } = null!;
    }
}
