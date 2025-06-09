namespace PizzaApp.Data.Models
{
    using MappingEntities;

    public class Drink
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<OrderDrink> OrdersDrinks { get; set; }
            = new HashSet<OrderDrink>();

    }
}
