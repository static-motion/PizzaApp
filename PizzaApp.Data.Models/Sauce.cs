﻿namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Models.MappingEntities;

    [Comment("All the sauces offered.")]
    public class Sauce : ISoftDeletable, IEntity<int>
    {
        [Comment("Primary Key unique identifier")]
        public int Id { get; set; }

        [Comment("Sauce type (tomato, pesto etc.)")]
        public string Type { get; set; } = null!;

        [Comment("Short sauce description")]
        public string Description { get; set; } = null!;

        [Comment("Current sauce price")]
        public decimal Price { get; set; }

        public ICollection<Pizza> Pizzas { get; set; } 
            = new HashSet<Pizza>();

        [Comment("Shows if the entity is active.")]
        public bool IsDeleted { get; set; }

        public ICollection<OrderPizza> SauceOrders { get; set; }
            = new HashSet<OrderPizza>();
    }
}
