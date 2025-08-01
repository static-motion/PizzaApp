namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Orders;
    using PizzaApp.Web.ViewModels.ShoppingCart;

    public interface IOrderService
    {
        public Task<IEnumerable<OrderViewWrapper>> GetOrdersAsync(Guid userId);
        Task PlaceOrderAsync(OrderDetailsInputModel orderDetails, Guid userId);
    }
}
