namespace PizzaApp.Data.Models.MappingEntities
{
    using Microsoft.EntityFrameworkCore;

    [Comment("A many-to-many mapping entity between Pizza and Toppings, used to show which toppings are contained in which pizzas.")]
    public class PizzaTopping
    {
        [Comment("Foreign Key to Pizzas, part of composite Primary Key.")]
        public int PizzaId { get; set; }
        public required Pizza Pizza { get; set; }

        [Comment("Foreign Key to Toppings, part of composite Primary Key.")]
        public int ToppingId { get; set; }

        public required Topping Topping { get; set; }
    }
}