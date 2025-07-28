namespace PizzaApp.Data.Models.MappingEntities
{
    using Microsoft.EntityFrameworkCore;

    [Comment("A many-to-many mapping entity used to show which pizzas appear in which orders. ")]
    public class OrderPizza
    {
        [Comment("Primary Key for OrderPizza. ")]
        public Guid Id { get; set; }

        [Comment("Foreign Key to Orders. Shows which Order this pizza was used in.")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Comment("Foreign Key to Pizzas. This points to the original pizza the OrderPizza was based on.")]
        public int BasePizzaId { get; set; }
        public Pizza BasePizza { get; set; } = null!;

        [Comment("Quantity of pizzas in the order.")]
        public int Quantity { get; set; } = 1;

        [Comment("Price of the pizza at the time of purchase, used for total price calculations.")]
        public decimal PricePerItemAtPurchase { get; set; }

        [Comment("Dough used for this specific order pizza")]
        public int DoughId { get; set; }
        public Dough Dough { get; set; } = null!;

        [Comment("Sauce used for this specific order Pizza. Can be null.")]
        public int? SauceId { get; set; }
        public Sauce? Sauce { get; set; }

        [Comment("Toppings for this specific order Pizza")]
        public ICollection<OrderPizzaTopping> Toppings { get; set; } = new List<OrderPizzaTopping>();
    }
}