namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using PizzaApp.Data.Models;
    using PizzaApp.GCommon.Enums;
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
                .Property(e => e.PizzaType)
                .HasDefaultValue(PizzaType.AdminPizza)
                .IsRequired();

            entity
                .HasQueryFilter(e =>
                    e.IsDeleted == false // the pizza must be active
                    && (e.Sauce == null || e.Sauce.IsDeleted == false) // the sauce must be either not set or active
                    && e.Dough.IsDeleted == false // the dough must be active
                    && e.Toppings.All(t => t.Topping.IsDeleted == false // all toppings must be active
                    && t.Topping.ToppingCategory.IsDeleted == false) // all categories must be active
                );
        }
    }
}
