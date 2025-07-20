namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;

    using static PizzaApp.GCommon.EntityConstraints.Sauce;

    class SauceConfiguration : IEntityTypeConfiguration<Sauce>
    {
        public void Configure(EntityTypeBuilder<Sauce> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .Property(e => e.Type)
                .HasMaxLength(TypeMaxLength)
                .IsRequired();

            entity
                .Property(e => e.Description)
                .HasMaxLength(DescriptionMaxLength)
                .IsRequired();

            entity
                .Property(e => e.Price)
                .HasPrecision(6, 2)
                .HasSentinel(0m);

            entity
                .HasData(CreateSeed());

            entity
                .HasQueryFilter(e => e.IsDeleted == false);
        }

        private static List<Sauce> CreateSeed()
        {
            List<Sauce> sauces = 
            [
                new Sauce
                {
                    Id = 1,
                    Type = "Tomato",
                    Description = "Our signature tomato sauce with a special blend of herbs and spices that everyone knows and loves!",
                    Price = 1
                },
                new Sauce 
                {
                    Id = 2,
                    Type = "Cream",
                    Description = "Heavy cream sauce for rich and creamy pizzas. Did we mention it's very creamy?",
                    Price = 1
                },
                new Sauce 
                {
                    Id = 3,
                    Type = "BBQ",
                    Description = "Our custom made BBQ sauce with rich sweet and smokey aromas. Perfect for meaty pizzas!",
                    Price = 1
                },
                new Sauce
                {
                    Id = 4,
                    Type = "Pesto",
                    Description = "Olive oil, basil and garlic. We DARE you to think of a better flavor combination!",
                    Price = 1
                }
            ];

            return sauces;
        }
    }
}
