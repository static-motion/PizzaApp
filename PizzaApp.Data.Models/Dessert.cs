namespace PizzaApp.Data.Models
{
    using MappingEntities;

    public class Dessert
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Descripion { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<OrderDessert> Orders { get; set; }
            = new HashSet<OrderDessert>();

    }
}
