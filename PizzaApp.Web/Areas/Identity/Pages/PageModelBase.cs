namespace PizzaApp.Web.Areas.Identity.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Security.Claims;

    public class PageModelBase : PageModel
    {
        protected Guid GetUserId()
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out Guid guidId) ? guidId : throw new Exception("User ID not found");
        }
    }
}
