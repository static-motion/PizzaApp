namespace PizzaApp.Web.ViewModels
{
    public class PizzaIngredientsViewWrapper
    {
        public IReadOnlyList<ToppingCategoryViewWrapper> ToppingCategories { get; set; } = null!;
        public IReadOnlyList<DoughViewModel> Doughs { get; set; } = null!;
        public IReadOnlyList<SauceViewModel> Sauces { get; set; } = null!;
    }
}
