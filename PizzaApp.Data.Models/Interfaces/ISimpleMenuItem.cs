namespace PizzaApp.Data.Models.Interfaces
{
    public interface ISimpleMenuItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
