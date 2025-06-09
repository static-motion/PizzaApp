namespace PizzaApp.Data.Models.MappingEntities
{
    public class OrderPizza
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int PizzaId { get; set; }

        public Pizza Pizza { get; set; } = null!;

        public int Quantity { get; set; } = 1;

        public decimal PricePerItemAtPurchase { get; set; }
    }
}