namespace PizzaApp.Web.ViewModels.ShoppingCart
{
    using PizzaApp.Web.ViewModels.Address;

    public class ShoppingCartViewModel
    {
        public ShoppingCartItemsViewModel Items { get; set; } = null!;

        public OrderDetailsInputModel OrderDetails { get; set; } = null!;
        
        public IEnumerable<AddressViewModel> Addresses { get; set; } 
            = new List<AddressViewModel>();
    }
}
