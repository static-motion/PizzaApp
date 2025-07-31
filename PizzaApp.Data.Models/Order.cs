namespace PizzaApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models.Interfaces;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.GCommon.Enums;

    [Comment("All the users' orders in the database.")]
    public class Order : IEntity<Guid>
    {
        [Comment("Primary Key unique identifier")]
        public Guid Id { get; set; }

        [Comment("Foreign Key to Users - the user who made the order.")]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        [Comment("Current status of the order.")]
        public OrderStatus OrderStatus { get; set; }

        [Comment("Price of the order.")]
        public decimal Price { get; set; }

        [Comment("Date and time at which the order was created.")]
        public DateTime CreatedOn { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string? Comment { get; set; }

        public ICollection<OrderDessert> OrderDeserts { get; set; }
            = new List<OrderDessert>();

        public ICollection<OrderDrink> OrderDrinks { get; set; }
            = new List<OrderDrink>();

        public ICollection<OrderPizza> OrderPizzas { get; set; }
            = new List<OrderPizza>();

        [Comment("Foreign Key to Addresses - location where the order was supposed to be delivered.")]
        public int AddressId { get; set; }

        public Address DeliveryAddress { get; set; } = null!;
    }
}