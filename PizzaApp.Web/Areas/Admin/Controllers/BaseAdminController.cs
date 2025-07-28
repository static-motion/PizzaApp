namespace PizzaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PizzaApp.Web.Controllers;

    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class BaseAdminController : BaseController
    {
    }
}
