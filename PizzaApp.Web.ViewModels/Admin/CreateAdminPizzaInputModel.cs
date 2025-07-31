namespace PizzaApp.Web.ViewModels.Admin
{
    public class EditAdminPizzaInputModel
    {
        public PizzaIngredientsViewModel Ingredients { get; set; } = null!;
        public AdminPizzaInputModel Pizza { get; set; } = null!;
    }
}
