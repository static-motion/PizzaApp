namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Sauces")]
    public class Sauce
    {
        [Key]
        [Comment("Unique identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Sauce type")]
        public required string Type { get; set; }

        [Required]
        [Comment("Sauce description")]
        public required string Description { get; set; }

        [Comment("Sauce Price")]
        public decimal Price { get; set; }

        public ICollection<Pizza> Pizzas { get; set; } 
            = new HashSet<Pizza>();
    }
}
