namespace PizzaApp.Web.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;

    public class AddressesModel : PageModel
    {
        // TODO: Use UserService to access addresses.
        private readonly UserManager<User> _userManager;

        public AddressesModel(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<Address> SavedAddresses { get; set; } = new();

        public List<SelectListItem> AvailableCities { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string City { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Address Line 1")]
            public string AddressLine1 { get; set; } = string.Empty;

            [Display(Name = "Address Line 2")]
            public string? AddressLine2 { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await this.LoadAddressesAsync();
            await this.LoadCitiesAsync();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Guid userId = this.GetUserId();
            User user = await this.GetUserWithAddressesById(userId);

            if (!this.ModelState.IsValid)
            {
                await this.LoadAddressesAsync();
                await this.LoadCitiesAsync();
                return this.Page();
            }

            var address = new Address
            {
                City = this.Input.City,
                AddressLine1 = this.Input.AddressLine1,
                AddressLine2 = this.Input.AddressLine2,
                UserId = userId
            };

            user.Addresses.Add(address);
            IdentityResult result = await this._userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception($"Unable to save address for user with ID {userId}.");
            }

            return this.RedirectToPage();
        }

        private async Task<User> GetUserWithAddressesById(Guid userId)
        {
            return await _userManager.Users
                            .Include(u => u.Addresses)
                            .FirstOrDefaultAsync(u => u.Id == userId)
                            ?? throw new Exception($"Unable to load user with ID '{userId}'.");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Guid userId = this.GetUserId();
            User user = await this.GetUserWithAddressesById(userId);
            Address? address = user.Addresses
                .FirstOrDefault(a => a.Id == id && a.UserId == userId && !a.IsDeleted);

            if (address != null)
            {
                address.IsDeleted = true;
                await this._userManager.UpdateAsync(user);
            }

            return this.RedirectToPage();
        }

        private async Task LoadAddressesAsync()
        {
            var userId = this.GetUserId();

            SavedAddresses = (await _userManager.Users.Include(user => user.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId))?.Addresses.ToList() ?? new List<Address>();
        }

        private async Task LoadCitiesAsync()
        {
            // TODO: Replace with a service call to fetch cities dynamically
            this.AvailableCities =
            [
                new() { Text = "Plovdiv", Value = "Plovdiv" },
                new() { Text = "Varna", Value = "Varna" },
                new() { Text = "Sofia", Value = "Sofia" },
            ];
        }

        private Guid GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(id, out var guid) ? guid : throw new Exception("User ID not found");
        }
    }
}