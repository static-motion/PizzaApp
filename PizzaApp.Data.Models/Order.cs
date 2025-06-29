namespace PizzaApp.Data.Models
{
    using PizzaApp.Data.Common.Enums;
    using PizzaApp.Data.Models.MappingEntities;

    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public OrderStatus OrderStatus { get; set; }

        public decimal Price { get; set; }

        public ICollection<OrderDessert> OrderDeserts { get; set; }
            = new List<OrderDessert>();

        public ICollection<OrderDrink> OrderDrinks { get; set; }
            = new List<OrderDrink>();

        public ICollection<OrderPizza> OrderPizzas { get; set; }
            = new List<OrderPizza>();

        public int AddressId { get; set; }

        public Address DeliveryAddress { get; set; } = null!;
    }
}