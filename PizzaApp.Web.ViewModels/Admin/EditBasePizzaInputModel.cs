namespace PizzaApp.Web.ViewModels.Admin
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Interfaces;
    using PizzaApp.Web.ViewModels.Pizzas;

    public class EditBasePizzaInputModel : PizzaInputModelBase, IEditPizzaInputModel
    {
        public int Id { get; set; }

        public override PizzaType PizzaType => PizzaType.AdminPizza;

        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; }
    }
}
