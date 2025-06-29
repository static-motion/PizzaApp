namespace PizzaApp.Data.Models.MappingEntities
{
    using Microsoft.EntityFrameworkCore;

    [Comment("A many-to-many mapping entity used to show which pizzas appear in which orders.")]
    public class OrderPizza
    {
        [Comment("Foreign Key to Orders, part of composite Primary Key.")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Comment("Foreign Key to Pizzas, part of composite Primary Key.")]
        public int PizzaId { get; set; }

        public Pizza Pizza { get; set; } = null!;

        [Comment("Quantity of pizzas in the order.")]
        public int Quantity { get; set; } = 1;

        [Comment("Price of the pizza at the time of purchase, used for total price calculations.")]
        public decimal PricePerItemAtPurchase { get; set; }
    }
}