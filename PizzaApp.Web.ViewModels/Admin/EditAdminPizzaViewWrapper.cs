namespace PizzaApp.Web.ViewModels.Admin
{
    public class EditAdminPizzaViewWrapper
    {
        public PizzaIngredientsViewWrapper Ingredients { get; set; } = null!;
        public AdminPizzaInputModel Pizza { get; set; } = null!;
    }
}
