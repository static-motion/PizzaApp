namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;

    [Comment("All the dough types used for making pizzas.")]
    public class Dough : ISoftDeletable
    {
        [Comment("Unique identifier")]
        public int Id { get; set; }

        [Comment("Dough type")]
        public required string Type { get; set; }

        [Comment("Dough description")]
        public required string Description { get; set; }

        [Comment("Dough price")]
        public decimal Price { get; set; }

        public ICollection<Pizza> Pizzas { get; set; } 
            = new HashSet<Pizza>();

        [Comment("Shows if the entity is active.")]
        public bool IsDeleted { get; set; }
    }
}
