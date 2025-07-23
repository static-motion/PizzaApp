namespace PizzaApp.Data.Models.MappingEntities
{
    public class ShoppingCartDessert
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int DessertId { get; set; }
        public Dessert Dessert { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
