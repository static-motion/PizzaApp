namespace PizzaApp.Data.Models.MappingEntities
{
    using Microsoft.EntityFrameworkCore;

    [Comment("A many-to-many mapping entity between Pizza and Toppings, used to show which toppings are contained in which pizzas.")]
    public class PizzaTopping
    {
        [Comment("Foreign Key to Pizzas, part of composite Primary Key.")]
        public int PizzaId { get; set; }
        public virtual Pizza Pizza { get; set; } = null!;

        [Comment("Foreign Key to Toppings, part of composite Primary Key.")]
        public int ToppingId { get; set; }

        public virtual Topping Topping { get; set; } = null!;
    }
}