namespace PizzaApp.Data.Models.MappingEntities
{
    public class ShoppingCartDessert
    {
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;

        public int DessertId { get; set; }
        public Dessert Dessert { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
