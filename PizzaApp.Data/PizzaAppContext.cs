namespace PizzaApp.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using System.Reflection;

    public class PizzaAppContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Dessert> Desserts { get; set; }

        public DbSet<Dough> Doughs { get; set; }

        public DbSet<Drink> Drinks { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Pizza> Pizzas { get; set; }

        public DbSet<Sauce> Sauces { get; set; }

        public DbSet<Topping> Toppings { get; set; }

        public DbSet<ToppingCategory> ToppingCategories { get; set; }

        public DbSet<OrderDessert> OrdersDesserts { get; set; }

        public DbSet<PizzaTopping> PizzasToppings { get; set; }

        public DbSet<OrderDrink> OrdersDrinks { get; set; }

        public DbSet<OrderPizza> OrdersPizzas { get; set; }

        public DbSet<UserPizza> UsersPizzas { get; set; }

        public DbSet<ShoppingCartDrink> ShoppingCartsDrinks { get; set; }

        public DbSet<ShoppingCartDessert> ShoppingCartsDesserts { get; set; }

        public DbSet<ShoppingCartPizza> ShoppingCartsPizzas { get; set; }

        public bool BypassToppingFilters { get; set; } = false;

        public PizzaAppContext(DbContextOptions<PizzaAppContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Topping>()
                .HasQueryFilter(e => this.BypassToppingFilters ||
                    (e.IsDeleted == false && e.ToppingCategory.IsDeleted == false));

            modelBuilder.Entity<ToppingCategory>()
                .HasQueryFilter(e => this.BypassToppingFilters ||
                    (e.IsDeleted == false));
        }
    }
}
