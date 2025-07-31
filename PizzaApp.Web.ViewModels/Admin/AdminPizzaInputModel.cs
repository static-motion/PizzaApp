namespace PizzaApp.Web.ViewModels.Admin
{
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Web.ViewModels.Pizzas;
    public class AdminPizzaInputModel : PizzaInputModel
    {
        public int Id { get; set; }

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public override PizzaType PizzaType => PizzaType.BasePizza;

        public HashSet<int> SelectedToppingIds { get; set; } = new();

        public bool IsDeleted { get; set; }
    }
}
