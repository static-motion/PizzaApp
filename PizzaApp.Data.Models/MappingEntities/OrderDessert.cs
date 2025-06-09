namespace PizzaApp.Data.Models.MappingEntities
{
    using PizzaApp.Data.Models;

    public class OrderDessert
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int DessertId { get; set; }
        public Dessert Dessert { get; set; } = null!;

        public int Quantity { get; set; } = 1;

        public decimal PricePerItemAtPurchase { get; set; }
    }
}
