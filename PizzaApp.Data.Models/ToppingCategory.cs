namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;

    [Comment("The topping categories offered by the pizza app (meats, veggies etc.)")]
    public class ToppingCategory : ISoftDeletable, IEntity<int>
    {
        [Comment("Unique identifier")]
        public int Id { get; set; }

        [Comment("Topping type name")]
        public required string Name { get; set; }

        // All the topics from a certain category - (meats, vegetables etc.)
        public ICollection<Topping> Toppings { get; set; }
            = new HashSet<Topping>();

        [Comment("Shows if the entity is active.")]
        public bool IsDeleted { get; set; }
    }
}
