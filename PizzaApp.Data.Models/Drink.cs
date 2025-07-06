namespace PizzaApp.Data.Models
{
    using MappingEntities;
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;

    [Comment("All the drinks offered.")]
    public class Drink : ISoftDeletable
    {
        [Comment("Primary Key unique identifier")]
        public int Id { get; set; }

        [Comment("Name of the drink.")]
        public required string Name { get; set; }

        [Comment("Short description of the drink.")]
        public string? Description { get; set; }

        [Comment("Current Price of the drink.")]
        public decimal Price { get; set; }

        [Comment("URL for the image of the drink.")]
        public string? ImageUrl { get; set; }

        public ICollection<OrderDrink> OrdersDrinks { get; set; }
            = new HashSet<OrderDrink>();

        [Comment("Shows if the entity is active.")]
        public bool IsDeleted { get; set; }
    }
}
