namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ManageController : BaseAdminController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
