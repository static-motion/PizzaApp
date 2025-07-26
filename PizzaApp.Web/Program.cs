using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaApp.Data;
namespace PizzaApp.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository;
    using PizzaApp.Services.Core;
    using PizzaApp.Services.Core.Interfaces;
    using System.Threading.Tasks;

    using static PizzaApp.Web.Infrastructure.Extensions.ServiceCollectionExtensions;
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
            
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services
                .AddDbContext<PizzaAppContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                .AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 3;
                })
                .AddDefaultUI()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<PizzaAppContext>();

            /*builder.Services.AddRazorPages();*/

            builder.Services.AddRazorPages(options =>
            {
                // Customize specific Identity routes
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/Login");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "/Register");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Logout", "/Logout");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Index", "/Profile");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Email", "/Profile/Email");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Addresses", "/Profile/Addresses");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/PersonalData", "/Profile/PersonalData");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ChangePassword", "/Profile/ChangePassword");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/DeletePersonalData", "/Profile/DeletePersonalData");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/EnableAuthenticator", "/Profile/EnableAuthenticator");
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/TwoFactorAuthentication", "/Profile/TwoFactorAuthentication");
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddCustomServices(typeof(MenuService).Assembly);
            builder.Services.AddRepositories(typeof(PizzaRepository).Assembly);

            WebApplication? app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IUserSeedingService userSeeding = scope.ServiceProvider.GetRequiredService<IUserSeedingService>();
                await userSeeding.SeedUsersAndRolesAsync();
            }
            
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
