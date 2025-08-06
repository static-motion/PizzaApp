namespace PizzaApp.Web.ViewModels.MyPizzas
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Interfaces;
    using PizzaApp.Web.ViewModels.Pizzas;

    public class EditCustomerPizzaInputModel : PizzaInputModelBase, IEditPizzaInputModel
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public override PizzaType PizzaType => PizzaType.CustomerPizza;
    }
}
