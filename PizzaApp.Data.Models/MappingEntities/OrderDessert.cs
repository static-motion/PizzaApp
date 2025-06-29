namespace PizzaApp.Data.Models.MappingEntities
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;

    [Comment("A many-to-many mapping entity used to show which desserts appear in which orders.")]
    public class OrderDessert
    {
        [Comment("Foreign Key to Orders, part of composite Primary Key.")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Comment("Foreign Key to Desserts, part of composite Primary Key.")]
        public int DessertId { get; set; }
        public Dessert Dessert { get; set; } = null!;

        
        [Comment("Dessert quantity ordered.")]
        public int Quantity { get; set; } = 1;

        [Comment("Price of the pizza at the time of purchase, used for total price calculations.")]
        public decimal PricePerItemAtPurchase { get; set; }
    }
}
