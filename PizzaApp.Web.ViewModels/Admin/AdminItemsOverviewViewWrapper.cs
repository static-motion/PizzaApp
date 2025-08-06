namespace PizzaApp.Web.ViewModels.Admin
{
    using System.Collections.Generic;

    public class AdminItemsOverviewViewWrapper
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; } 

        public required string Category { get; set; }

        public IEnumerable<MenuItemViewModel> Items { get; set; } = new List<MenuItemViewModel>();
    }
}
