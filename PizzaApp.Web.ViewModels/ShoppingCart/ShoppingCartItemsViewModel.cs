namespace PizzaApp.Web.ViewModels.ShoppingCart
{
    public class ShoppingCartItemsViewModel
    {
        public IEnumerable<ShoppingCartPizzaViewModel> Pizzas { get; set; } 
            = new List<ShoppingCartPizzaViewModel>();
        public IEnumerable<ShoppingCartDrinkViewModel> Drinks { get; set; } 
            = new List<ShoppingCartDrinkViewModel>();
        public IEnumerable<ShoppingCartDessertViewModel> Desserts { get; set; }
            = new List<ShoppingCartDessertViewModel>();

        public decimal GrandTotal
        {
            get
            {
                decimal total = 0;
                total += this.Pizzas.Sum(p => p.Price * p.Quantity);
                total += this.Drinks.Sum(d => d.Price * d.Quantity);
                total += this.Desserts.Sum(d => d.Price * d.Quantity);
                return total;
            }
        }
    }
}
