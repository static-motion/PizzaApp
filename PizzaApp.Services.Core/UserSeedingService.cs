﻿namespace PizzaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;

    public class UserSeedingService : IUserSeedingService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public UserSeedingService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, IUserRepository cartRepository)
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
            {
                return;
            }
            admin = new()
            {
                Email = adminEmail,
                UserName = adminEmail,
                EmailConfirmed = true,
            };
            IdentityResult result = await this._userManager.CreateAsync(admin, adminPassword);
            await this._userManager.AddToRoleAsync(admin, "Admin");
            if (!result.Succeeded)
            {
                // TODO: handle
            }
        }


        private async Task SeedRolesAsync()
        {
            var roles = new[] { "Admin", "User", "Manager", "Staff" };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole<Guid>(roleName);
                    var result = await _roleManager.CreateAsync(role);

                    // TODO: HANDLE
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
