namespace PizzaApp.Data.Models.MappingEntities
{
    public class OrderDrink
    {
        public Guid OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public int DrinkId { get; set; }

        public Drink Drink { get; set; } = null!;

        public int Quantity { get; set; } = 1;

        public decimal PricePerItemAtPurchase { get; set; }
    }
}
