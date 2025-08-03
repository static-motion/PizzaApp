namespace PizzaApp.Web.ViewModels.Menu
{
    using PizzaApp.GCommon.Enums;

    public class MenuCategoryViewWrapper
    {
        public IEnumerable<MenuItemViewModel> Items { get; set; } = null!;

        public Dictionary<MenuCategory, string> AllCategories { get; set; } = null!;

        public MenuCategory Category { get; set; }
    }
}
