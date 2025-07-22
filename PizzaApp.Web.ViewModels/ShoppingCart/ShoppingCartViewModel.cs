namespace PizzaApp.Web.ViewModels.ShoppingCart
{
    using PizzaApp.Web.ViewModels.Pizzas;

    public class ShoppingCartViewModel
    {
        public ICollection<PizzaInputModel>? MyProperty { get; set; }
    }
}
