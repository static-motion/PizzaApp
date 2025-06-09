using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.Data.Models.MappingEntities
{
    public class UserPizza
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; } = null!;
    }
}