namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Models.MappingEntities;

    [Comment("All the dough types used for making pizzas.")]
    public class Dough : ISoftDeletable, IEntity<int>
    {
        [Comment("Unique identifier")]
        public int Id { get; set; }

        [Comment("Dough type")]
        public string Type { get; set; } = null!;

        [Comment("Dough description")]
        public string Description { get; set; } = null!;

        [Comment("Dough price")]
        public decimal Price { get; set; }

        public ICollection<Pizza> Pizzas { get; set; } 
            = new HashSet<Pizza>();

        [Comment("Shows if the entity is active.")]
        public bool IsDeleted { get; set; }
        public ICollection<OrderPizza> DoughOrders { get; set; } 
            = new HashSet<OrderPizza>();
    }
}
