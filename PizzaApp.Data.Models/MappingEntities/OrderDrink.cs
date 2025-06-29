namespace PizzaApp.Data.Models.MappingEntities
{
    using Microsoft.EntityFrameworkCore;

    [Comment("A many-to-many mapping entity used to show which drinks appear in which orders.")]
    public class OrderDrink
    {
        [Comment("Foreign Key to Orders, part of composite Primary Key.")]
        public Guid OrderId { get; set; }

        public Order Order { get; set; } = null!;

        [Comment("Foreign Key to Drinks, part of composite Primary Key.")]
        public int DrinkId { get; set; }

        public Drink Drink { get; set; } = null!;

        [Comment("Ordered drink quantity")]
        public int Quantity { get; set; } = 1;

        [Comment("Price of the drink at the time of purchase, used for total price calculations.")]
        public decimal PricePerItemAtPurchase { get; set; }
    }
}
