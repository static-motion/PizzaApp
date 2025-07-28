namespace PizzaApp.Web.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Address;

    public class AddressesModel : PageModelBase
    {
        // TODO: Use UserService to access addresses.
        private readonly IUserService _userService;

        public AddressesModel(IUserService userService)
        {
            this._userService = userService;
        }

        [BindProperty]
        public AddressInputModel Input { get; set; } = new();

        public IEnumerable<AddressViewModel> SavedAddresses { get; set; } = null!;

        public IEnumerable<SelectListItem> AvailableCities { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            await this.LoadAddressesAsync();
            await this.LoadCitiesAsync();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                await this.LoadAddressesAsync();
                await this.LoadCitiesAsync();
                return this.Page();
            }

            Guid userId = this.GetUserId();
            await this._userService.AddAddressAsync(userId, this.Input);
            return this.RedirectToPage();
        }

        // TODO: Use UserService to access addresses.
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Guid userId = this.GetUserId();
            await this._userService.DeleteAddressAsync(userId, id);
            
            return this.RedirectToPage();
        }

        private async Task LoadAddressesAsync()
        {
            Guid userId = this.GetUserId();
            this.SavedAddresses = await this._userService.GetUserAddressesAsync(userId);
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
    }
}