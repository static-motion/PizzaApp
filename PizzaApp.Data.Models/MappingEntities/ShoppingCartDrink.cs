namespace PizzaApp.Data.Models.MappingEntities
{
    public class ShoppingCartDrink
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int DrinkId { get; set; }
        public Drink Drink { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
