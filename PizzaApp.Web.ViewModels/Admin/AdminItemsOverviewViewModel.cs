namespace PizzaApp.Web.ViewModels.Admin
{
    using PizzaApp.GCommon.Enums;
    using System.Collections.Generic;

    public class AdminItemsOverviewViewModel
    {
        public ManagementCategory Category { get; set; }

        public IEnumerable<MenuItemViewModel> Items { get; set; } = new List<MenuItemViewModel>();
    }
}
