namespace PizzaApp.Data.Models.MappingEntities
{
    public class PizzaTopping
    {
        public int PizzaId { get; set; }
        public required Pizza Pizza { get; set; }

        public int ToppingId { get; set; }
        public required Topping Topping { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal PricePerItemAtPurchase { get; set; }
    }
}