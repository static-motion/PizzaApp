namespace PizzaApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using PizzaApp.Data.Models.MappingEntities;

    public class User : IdentityUser<Guid>
    {
        public ICollection<Address> Addresses { get; set; }
            = new List<Address>();

        public ICollection<UserPizza> FavoritePizzas { get; set; }
            = new List<UserPizza>();

        public ICollection<Pizza> CreatedPizzas { get; set; }
            = new List<Pizza>();

        public ICollection<Order> OrderHistory { get; set; }
            = new HashSet<Order>();
    }
}