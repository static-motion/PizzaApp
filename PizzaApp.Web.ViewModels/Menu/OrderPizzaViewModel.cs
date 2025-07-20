namespace PizzaApp.Web.ViewModels.Menu
{
    public class OrderPizzaViewModel
    {
        public PizzaDetailsViewModel Pizza { get; set; } = null!;
        public IReadOnlyList<ToppingCategoryViewModel> ToppingCategories { get; set; } = null!;
        public IReadOnlyList<DoughViewModel> Doughs { get; set; } = null!;
        public IReadOnlyList<SauceViewModel> Sauces { get; set; } = null!;
    }
}

