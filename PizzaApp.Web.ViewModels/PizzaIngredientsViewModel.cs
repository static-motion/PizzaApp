namespace PizzaApp.Web.ViewModels
{
    public class PizzaIngredientsViewModel
    {
        public IReadOnlyList<ToppingCategoryViewModel> ToppingCategories { get; set; } = null!;
        public IReadOnlyList<DoughViewModel> Doughs { get; set; } = null!;
        public IReadOnlyList<SauceViewModel> Sauces { get; set; } = null!;
    }
}
