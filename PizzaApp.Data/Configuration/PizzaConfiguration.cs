namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using PizzaApp.Data.Models;

    using static PizzaApp.GCommon.EntityConstraints.Pizza;

    class PizzaConfiguration : IEntityTypeConfiguration<Pizza>
    {
        public void Configure(EntityTypeBuilder<Pizza> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .Property(e => e.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired();

            entity
                .Property(e => e.Description)
                .HasMaxLength(DescriptionMaxLength)
                .IsRequired(false);

            entity
                .HasOne(e => e.Dough)
                .WithMany(d => d.Pizzas)
                .HasForeignKey(e => e.DoughId)
                .IsRequired();

            entity
                .HasOne(e => e.Sauce)
                .WithMany(s => s.Pizzas)
                .HasForeignKey(e => e.SauceId);

            entity
                .Property(e => e.ImageUrl)
                .HasMaxLength(ImageUrlMaxLength)
                .IsRequired(false);

            entity
                .HasOne(e => e.Creator)
                .WithMany(u => u.CreatedPizzas)
                .HasForeignKey(e => e.CreatorUserId)
                .IsRequired();

            entity
                .HasData(GeneratePizzaSeed());

            entity
                .HasQueryFilter(e =>
                    e.IsDeleted == false // the pizza must be active
                    && (e.Sauce == null || e.Sauce.IsDeleted == false) // the sauce must be either not set or active
                    && e.Dough.IsDeleted == false // the dough must be active
                    && e.Toppings.All(t => t.Topping.IsDeleted == false) // all toppings must be active
                );
        }

        private static IEnumerable<Pizza> GeneratePizzaSeed()
        {
            return
            [
                new Pizza
                {
                    Id = 1,
                    Name = "Classic Pepperoni",
                    Description = "Tomato sauce, mozzarella and pepperoni on white dough. Simple. Classic. Timeless.",
                    CreatorUserId = Guid.Parse("7BC9CF3B-7464-4B4A-EA3B-08DDB8A10943"),
                    SauceId = 1,
                    DoughId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1716237389720-2c4fdabf0ac0?q=80&w=2574&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                }
            ];
        }
    }
}
