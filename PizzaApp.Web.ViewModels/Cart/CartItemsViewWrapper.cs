namespace PizzaApp.Web.ViewModels.ShoppingCart
{
    public class CartItemsViewWrapper
    {
        public IEnumerable<CartPizzaViewModel> Pizzas { get; set; } 
            = new List<CartPizzaViewModel>();
        public IEnumerable<CartDrinkViewModel> Drinks { get; set; } 
            = new List<CartDrinkViewModel>();
        public IEnumerable<CartDessertViewModel> Desserts { get; set; }
            = new List<CartDessertViewModel>();

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
