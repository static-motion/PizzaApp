namespace PizzaApp.Data.Models
{
    using MappingEntities;
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;

    [Comment("All the desserts offered.")]
    public class Dessert : ISoftDeletable, IEntity<int>
    {
        [Comment("Primary Key unique identifier.")]
        public int Id { get; set; }

        [Comment("Name of the dessert")]
        public string Name { get; set; } = null!;

        [Comment("Short description of the dessert.")]
        public string? Description { get; set; }

        [Comment("Current price of the dessert.")]
        public decimal Price { get; set; }

        [Comment("URL for the image of the dessert.")]
        public string? ImageUrl { get; set; }

        public ICollection<OrderDessert> Orders { get; set; }
            = new HashSet<OrderDessert>();

        [Comment("Shows if the entity is active.")]
        public bool IsDeleted { get; set; }
        public ICollection<ShoppingCartDessert> ShoppingCarts { get; set; }
            = new HashSet<ShoppingCartDessert>();
    }
}
