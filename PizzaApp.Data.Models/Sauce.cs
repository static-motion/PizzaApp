namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("All the sauces offered.")]
    public class Sauce
    {
        [Comment("Primary Key unique identifier")]
        public int Id { get; set; }

        [Comment("Sauce type (tomato, pesto etc.)")]
        public required string Type { get; set; }

        [Comment("Short sauce description")]
        public required string Description { get; set; }

        [Comment("Current sauce price")]
        public decimal Price { get; set; }

        public ICollection<Pizza> Pizzas { get; set; } 
            = new HashSet<Pizza>();
    }
}
