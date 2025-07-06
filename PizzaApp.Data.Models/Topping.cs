namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Models.MappingEntities;

    [Comment("All the toppings offered.")]
    public class Topping : ISoftDeletable
    {
        [Comment("Primary Key unique identifier")]
        public int Id { get; set; }

        [Comment("Foreign key to topping categories, shows which category the topping belongs to (meats, veggies etc.)")]
        public int ToppingCategoryId { get; set; }

        public ToppingCategory ToppingCategory { get; set; } = null!;

        [Comment("Name of the pizza topping")]
        public required string Name { get; set; }

        [Comment("A short description of the pizza topping.")]
        public required string Description { get; set; }

        [Comment("Current price of the pizza topping")]
        public decimal Price { get; set; }

        public ICollection<PizzaTopping> PizzasToppings { get; set; }
            = new HashSet<PizzaTopping>();

        [Comment("Shows if the entity is active.")]
        public bool IsDeleted { get; set; }
    }
}
