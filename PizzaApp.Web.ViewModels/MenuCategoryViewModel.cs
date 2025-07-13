namespace PizzaApp.Web.ViewModels
{
    using PizzaApp.GCommon.Enums;

    public class MenuCategoryViewModel
    {
        public MenuCategory Category { get; set; }

        public IEnumerable<MenuItemViewModel> Items { get; set; } = new List<MenuItemViewModel>();

        public IEnumerable<string> AllCategories = new List<string>();
    }
}
