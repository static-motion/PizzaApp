namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class AccountController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return this.View();
        }
    }
}
