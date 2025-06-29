namespace PizzaApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.MappingEntities;

    [Comment("The general public website user. This entity has addresses, created pizzas, favorited pizzas and order associated with it.")]
    public class User : IdentityUser<Guid>
    {
        // "A list of the User's addresses.
        public ICollection<Address> Addresses { get; set; }
            = new List<Address>();

        // A list of the User's favorite pizzas.
        public ICollection<UserPizza> FavoritePizzas { get; set; }
            = new List<UserPizza>();

        // A list of the pizzas the user has created.
        public ICollection<Pizza> CreatedPizzas { get; set; }
            = new List<Pizza>();

        // Every order the User has made.
        public ICollection<Order> OrderHistory { get; set; }
            = new HashSet<Order>();
    }
}