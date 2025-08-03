namespace PizzaApp.Web.Controllers
{
    using System.Diagnostics;

    using ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger)
        {

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? code)
        {
            return code switch
            {
                400 => this.View("400BadRequest"),
                401 => this.View("401Unauthorized"),
                403 => this.View("403Forbidden"),
                404 => this.View("404NotFound"),
                500 => this.View("500ServerError"),
                _ => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier })
            };
        }
    }
}
