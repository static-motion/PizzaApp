namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    public class ToppingCategory
    {
        [Comment("Unique identifier")]
        public int Id { get; set; }

        [Comment("Topping type name")]
        public required string Name { get; set; }

        public ICollection<Topping> Toppings { get; set; }
            = new HashSet<Topping>();
    }
}
