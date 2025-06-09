namespace PizzaApp.Data.Models
{
    using PizzaApp.Data.Models.MappingEntities;

    public class Pizza
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<PizzaTopping> Toppings { get; set; }
            = new List<PizzaTopping>();

        public int DoughId { get; set; }
        public Dough Dough { get; set; } = null!;

        public int? SauceId { get; set; }
        public Sauce? Sauce { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CreatorUserId { get; set; }

        public User Creator { get; set; } = null!;

        public ICollection<UserPizza> FavoriteOf { get; set; }
            = new HashSet<UserPizza>();

        public ICollection<OrderPizza> Orders { get; set; }
            = new HashSet<OrderPizza>();
    }
}