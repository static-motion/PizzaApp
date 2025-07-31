namespace PizzaApp.Web.ViewModels.Admin
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
