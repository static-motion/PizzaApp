namespace PizzaApp.Data.Models.MappingEntities
{
    public class ShoppingCartDrink
    {
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;

        public int DrinkId { get; set; }
        public Drink Drink { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
