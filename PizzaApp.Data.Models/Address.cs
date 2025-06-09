namespace PizzaApp.Data.Models
{
    public class Address
    {
        public int Id { get; set; }

        public required string City { get; set; }

        public required string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Order> Deliveries { get; set; }
            = new HashSet<Order>();

        public bool IsDeleted { get; set; }
    }
}
