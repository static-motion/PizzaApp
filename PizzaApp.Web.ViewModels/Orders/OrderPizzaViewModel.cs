namespace PizzaApp.Web.ViewModels.Orders
{
    public class OrderPizzaViewModel
    {
        public required string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public required string DoughName { get; set; }

        public string? SauceName { get; set; }

        public Dictionary<string, List<string>> Toppings { get; set; }
            = new Dictionary<string, List<string>>();
        
    }
}
