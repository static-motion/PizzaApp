namespace PizzaApp.Web.ViewModels.Menu
{
    public class OrderPizzaViewModel
    {
        public PizzaDetailsViewModel Pizza { get; set; } = null!;
        public IEnumerable<ToppingCategoryViewModel> ToppingCategories { get; set; } = null!;
        public IEnumerable<DoughViewModel> Doughs { get; set; } = null!;
        public IEnumerable<SauceViewModel> Sauces { get; set; } = null!;
    }
}

