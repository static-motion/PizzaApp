namespace PizzaApp.Data.Models
{
    using MappingEntities;
    using Microsoft.EntityFrameworkCore;

    [Comment("All the desserts offered.")]
    public class Dessert
    {
        [Comment("Primary Key unique identifier.")]
        public int Id { get; set; }

        [Comment("Name of the dessert")]
        public required string Name { get; set; }

        [Comment("Short description of the dessert.")]
        public string? Descripion { get; set; }

        [Comment("Current price of the dessert.")]
        public decimal Price { get; set; }

        [Comment("URL for the image of the dessert.")]
        public string? ImageUrl { get; set; }

        public ICollection<OrderDessert> Orders { get; set; }
            = new HashSet<OrderDessert>();

    }
}
