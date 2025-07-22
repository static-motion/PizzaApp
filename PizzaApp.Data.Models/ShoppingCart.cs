namespace PizzaApp.Data.Models
{
    using PizzaApp.Data.Models.MappingEntities;

    public class ShoppingCart
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<ShoppingCartPizza>? Pizzas { get; set; }
        public ICollection<ShoppingCartDrink>? Drinks { get; set; }
        public ICollection<ShoppingCartDessert>? Desserts { get; set; }
    }
}

