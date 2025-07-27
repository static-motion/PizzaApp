namespace PizzaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class BaseController : Controller
    {
        protected bool IsUserAuthenticated()
        {
            return this.User.Identity?.IsAuthenticated ?? false;
        }

        protected Guid? GetUserId()
        {
            string? id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out Guid guidId) ? guidId : null;
        }
    }
}
