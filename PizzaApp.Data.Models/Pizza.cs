namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.GCommon.Enums;

    [Comment("All pizzas offered - both admin and user created.")]
    public class Pizza : ISoftDeletable
    {
        [Comment("Primary Key unique identifier.")]
        public int Id { get; set; }

        [Comment("Name of the pizza as given by its creator (User)")]
        public required string Name { get; set; }

        [Comment("Short pizza description")]
        public string? Description { get; set; }

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

        public Pizza? BasePizza { get; set; }

        public int? BasePizzaId { get; set; }

        public PizzaType PizzaType { get; set; }

        public ICollection<UserPizza> FavoritedBy { get; set; }
            = new HashSet<UserPizza>();

        public ICollection<OrderPizza> Orders { get; set; }
            = new HashSet<OrderPizza>();

        public ICollection<Pizza> BaseOf { get; set; }
            = new HashSet<Pizza>();

        public ICollection<ShoppingCartPizza> ShoppingCarts { get; set; }
            = new HashSet<ShoppingCartPizza>();

        [Comment("Shows if the pizza has been soft deleted.")]
        public bool IsDeleted { get; set; }
    }
}