namespace PizzaApp.Data.Models.MappingEntities
{
    public class ShoppingCartPizza
    {
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;

        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
