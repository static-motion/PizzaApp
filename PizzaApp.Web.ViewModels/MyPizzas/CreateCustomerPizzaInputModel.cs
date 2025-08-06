namespace PizzaApp.Web.ViewModels.Pizzas
{
    using PizzaApp.GCommon.Enums;

    public class CreateCustomerPizzaInputModel : PizzaInputModelBase
    {
        public override PizzaType PizzaType => PizzaType.CustomerPizza;
    }
}
