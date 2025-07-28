namespace PizzaApp.Web.ViewModels.ShoppingCart
{
    public class CartDessertViewModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}