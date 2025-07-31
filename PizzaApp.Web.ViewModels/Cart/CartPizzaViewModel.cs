namespace PizzaApp.Web.ViewModels.ShoppingCart
{
    public class CartPizzaViewModel
    {
        public required int Id { get; set; }

        public required string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public required string DoughName { get; set; }

        public string? SauceName { get; set; }

        public Dictionary<string, List<ToppingViewModel>> Toppings { get; set; }
            = new Dictionary<string, List<ToppingViewModel>>();
    }
}