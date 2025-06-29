namespace PizzaApp.Data.Models.MappingEntities
{
    using Microsoft.EntityFrameworkCore;

    [Comment("A many-to-many mapping entity between User and Pizza, showing pizza entities which have been marked as favorite by users")]
    public class UserPizza
    {
        [Comment("Foreign Key to Users")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Comment("Foreign Key to Pizzas")]
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; } = null!;
    }
}