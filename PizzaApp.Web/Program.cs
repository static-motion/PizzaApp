namespace PizzaApp.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core;
    using PizzaApp.Services.Core.Interfaces;
    using System.Threading.Tasks;

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
                .AddDefaultIdentity<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 3;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<PizzaAppContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<UserSeedingService>();

            builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
            builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
            builder.Services.AddScoped<IDessertRepository, DessertRepository>();
            builder.Services.AddScoped<IDoughRepository, DoughRepository>();
            builder.Services.AddScoped<IToppingRepository, ToppingRepository>();
            builder.Services.AddScoped<ISauceRepository, SauceRepository>();

            builder.Services.AddScoped<IMenuService, MenuService>();


            WebApplication? app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                UserSeedingService userSeeding = scope.ServiceProvider.GetRequiredService<UserSeedingService>();
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
