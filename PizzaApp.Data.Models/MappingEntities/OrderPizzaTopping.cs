namespace PizzaApp.Data.Models.MappingEntities
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Toppings for a specific pizza in an order")]
    public class OrderPizzaTopping
    {
        [Comment("Foreign Key to OrderPizzas, part of composite Primary Key.")]
        public Guid OrderPizzaId { get; set; }

        public OrderPizza OrderPizza { get; set; } = null!;

        [Comment("Foreign Key to Toppings, part of composite Primary Key.")]
        public int ToppingId { get; set; }
        public Topping Topping { get; set; } = null!;

        [Comment("Price of the topping at the time of purchase")]
        public decimal PriceAtPurchase { get; set; }
    }
    
}
