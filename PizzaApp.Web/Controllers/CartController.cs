namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class CartController :  BaseController
    {
        public async Task<IActionResult> Index()
        {
            return this.View();
        }
    }
}
