namespace PizzaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using PizzaApp.Data.Models;

    public class UserSeedingService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public UserSeedingService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }


        public async Task SeedUsersAndRolesAsync()
        {
            await this.SeedRolesAsync();
            await this.SeedAdminAsync();
        }
        private async Task SeedAdminAsync()
        {
            //string adminUserName = "admin";
            string adminEmail = "admin@pizzahub.com";
            string adminPassword = "admin123";

            User? admin = await this._userManager.FindByEmailAsync(adminEmail);

            if (admin is not null)
                return; // await this._userManager.DeleteAsync(admin);

            admin = new()
            {
                Email = adminEmail,
                UserName = adminEmail,
                EmailConfirmed = true,
            };

            IdentityResult result = await this._userManager.CreateAsync(admin, adminPassword);
            if (!result.Succeeded)
            {
                // TODO: handle
            }
        }


        private async Task SeedRolesAsync()
        {
            var roles = new[] { "Admin", "User", "Manager" };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole<Guid>(roleName);
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        //_logger.LogInformation("Created role: {RoleName}", roleName);
                    }
                    else
                    {
                        //_logger.LogError("Failed to create role {RoleName}: {Errors}",
                        //roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
    }
}
