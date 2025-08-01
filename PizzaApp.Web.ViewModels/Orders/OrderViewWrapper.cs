namespace PizzaApp.Web.ViewModels.Orders
{
    using PizzaApp.GCommon.Enums;

    public class OrderViewWrapper
    {
        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<OrderPizzaViewModel> Pizzas { get; set; } 
            = new List<OrderPizzaViewModel>();

        public IEnumerable<OrderDrinkViewModel> Drinks { get; set; } 
            = new List<OrderDrinkViewModel>();

        public IEnumerable<OrderDessertViewModel> Desserts { get; set; }
            = new List<OrderDessertViewModel>();
    }
}
