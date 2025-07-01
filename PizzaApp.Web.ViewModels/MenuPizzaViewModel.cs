namespace PizzaApp.Web.ViewModels
{
    public class MenuPizzaViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}
