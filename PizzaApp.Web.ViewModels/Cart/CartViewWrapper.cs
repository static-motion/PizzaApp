namespace PizzaApp.Web.ViewModels.Cart
{
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using PizzaApp.Web.ViewModels.Address;

    public class CartViewWrapper
    {
        [ValidateNever]
        public CartItemsViewWrapper Items { get; set; } = null!;

        public OrderDetailsInputModel OrderDetails { get; set; } = null!;
        
        public IEnumerable<AddressViewModel> Addresses { get; set; } 
            = new List<AddressViewModel>();
    }
}
