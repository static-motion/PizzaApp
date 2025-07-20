namespace PizzaApp.Web.ViewModels.Menu
{
    using PizzaApp.GCommon.Enums;

    public class MenuItemViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public MenuCategory Category { get; set; }
    }
}
