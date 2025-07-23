namespace PizzaApp.Web.ViewModels
{
    public class ToppingViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public decimal Price { get; set; }
    }
}