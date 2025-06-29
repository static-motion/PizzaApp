namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.MappingEntities;

    [Comment("All pizzas offered - both admin and user created.")]
    // TODO: Add a flag to differentiate betwen user and admin made pizzas? Or use querying to find pizzas only created by admins (could be slow when there are a lot of pizzas)
    public class Pizza
    {
        [Comment("Primary Key unique identifier.")]
        public int Id { get; set; }

        [Comment("Name of the pizza as given by its creator (User)")]
        public required string Name { get; set; }

        public ICollection<PizzaTopping> Toppings { get; set; }
            = new List<PizzaTopping>();

        [Comment("The type of dough the pizza is made with.")]
        public int DoughId { get; set; }
        public Dough Dough { get; set; } = null!;

        [Comment("The sauce used on the pizza. Can be null.")]
        public int? SauceId { get; set; }
        public Sauce? Sauce { get; set; }

        [Comment("URL of the image of the pizza.")]
        public string? ImageUrl { get; set; }

        [Comment("Foreign Key to User who created the pizza.")]
        public Guid CreatorUserId { get; set; }

        public User Creator { get; set; } = null!;

        public ICollection<UserPizza> FavoriteOf { get; set; }
            = new HashSet<UserPizza>();

        public ICollection<OrderPizza> Orders { get; set; }
            = new HashSet<OrderPizza>();
    }
}