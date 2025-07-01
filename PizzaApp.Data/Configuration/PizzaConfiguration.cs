namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using static PizzaApp.Data.Common.EntityConstraints.Pizza;

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
        }

        private static IEnumerable<Pizza> GeneratePizzaSeed()
        {
            return
            [
                new Pizza
                {
                    Id = 1,
                    Name = "Classic Pepperoni",
                    CreatorUserId = Guid.Parse("7BC9CF3B-7464-4B4A-EA3B-08DDB8A10943"),
                    SauceId = 1,
                    DoughId = 1,
                }
            ];
        }
    }
}
