namespace PizzaApp.Web.ViewModels.Admin
{
    public class EditToppingViewWrapper
    {
        public EditToppingInputModel ToppingInputModel { get; set; } = null!;

        public required IEnumerable<ToppingCategorySelectViewModel> AllCategories { get; set; }
    }
}
