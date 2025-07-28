namespace YourApp.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.Areas.Identity.Pages;
    using PizzaApp.Web.ViewModels.Orders;

    public class PreviousOrdersModel : PageModelBase
    {
        private readonly IOrderService _orderService;

        public PreviousOrdersModel(IOrderService orderService)
        {
            this._orderService = orderService;
        }
        public IEnumerable<OrderViewModel> Orders { get; private set; } = new List<OrderViewModel>();
        public async Task<IActionResult> OnGetAsync()
        {
            this.Orders = (await this._orderService.GetOrdersAsync(this.GetUserId()))
                .OrderByDescending(o => o.CreatedOn);

            return this.Page();
        }
    }
}