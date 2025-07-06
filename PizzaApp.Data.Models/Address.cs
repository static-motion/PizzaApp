namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;

    [Comment("All the addresses as created by the Users.")]
    public class Address : ISoftDeletable
    {
        [Comment("Primary Key unique identifier.")]
        public int Id { get; set; }

        [Comment("The city where the address is located at.")]
        public required string City { get; set; }

        [Comment("First address line.")]
        public required string AddressLine1 { get; set; }

        [Comment("Second address line. Can be null.")]
        public string? AddressLine2 { get; set; }

        [Comment("Foreign Key to Users table - User who is associated with this address.")]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Order> Deliveries { get; set; }
            = new HashSet<Order>();

        [Comment("Shows if the address has been soft deleted.")]
        public bool IsDeleted { get; set; }
    }
}
