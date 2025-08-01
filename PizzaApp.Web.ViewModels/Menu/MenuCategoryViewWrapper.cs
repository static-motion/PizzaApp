namespace PizzaApp.Web.ViewModels.Menu
{
    public class MenuCategoryViewWrapper
    {
        public IEnumerable<MenuItemViewModel> Items { get; set; } = new List<MenuItemViewModel>();

        public IEnumerable<string> AllCategories = new List<string>();
    }
}
