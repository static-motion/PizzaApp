namespace PizzaApp.Web.ViewModels.Admin
{
    public class EditAdminPizzaViewWrapper
    {
        public PizzaIngredientsViewWrapper Ingredients { get; set; } = null!;
        public BasePizzaInputModel Pizza { get; set; } = null!;
    }
}
