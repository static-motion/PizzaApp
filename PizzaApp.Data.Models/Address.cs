namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;

    [Comment("All the addresses as created by the Users.")]
    public class Address : ISoftDeletable, IEntity<int>
    {
        [Comment("Primary Key unique identifier.")]
        public int Id { get; set; }

        [Comment("The city where the address is located at.")]
        public string City { get; set; } = null!;

        [Comment("First address line.")]
        public string AddressLine1 { get; set; } = null!;

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
