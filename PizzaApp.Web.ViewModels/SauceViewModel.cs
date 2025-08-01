namespace PizzaApp.Web.ViewModels
{
    public class SauceViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
