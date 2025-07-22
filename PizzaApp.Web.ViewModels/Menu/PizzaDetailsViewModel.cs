namespace PizzaApp.Web.ViewModels.Menu
{

    public class PizzaDetailsViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public int DoughId { get; set; }

        public int? SauceId { get; set; }

        public string? ImageUrl { get; set; }

        // calculated on post
        public decimal? Price { get; set; }

        public IEnumerable<int> SelectedToppingIds { get; set; } = null!;
    }
}
