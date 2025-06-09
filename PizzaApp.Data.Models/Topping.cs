namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.MappingEntities;

    public class Topping
    {
        [Comment("Unique identifier")]
        public int Id { get; set; }

        [Comment("Foreign key to topping types")]
        public int ToppingTypeId { get; set; }

        public ToppingCategory ToppingCategory { get; set; } = null!;

        [Comment("Topping name")]
        public required string Name { get; set; }

        [Comment("Topping description")]
        public required string Desicription { get; set; }

        [Comment("Topping price")]
        public decimal Price { get; set; }

        public ICollection<PizzaTopping> PizzasToppings { get; set; }
            = new HashSet<PizzaTopping>();
    }
}
