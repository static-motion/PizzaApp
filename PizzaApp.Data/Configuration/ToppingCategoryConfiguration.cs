namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;

    using static PizzaApp.Data.Common.EntityConstraints.ToppingType;

    class ToppingCategoryConfiguration : IEntityTypeConfiguration<ToppingCategory>
    {
        public void Configure(EntityTypeBuilder<ToppingCategory> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .Property(e => e.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired();

            entity
                .HasData(CreateSeed());
        }

        private static List<ToppingCategory> CreateSeed()
        {
            List<ToppingCategory> categories =
            [
                new ToppingCategory
                {
                    Id = 1,
                    Name = "Meats"
                },
                new ToppingCategory
                {
                    Id = 2,
                    Name = "Cheeses"
                },
                new ToppingCategory
                {
                    Id = 3,
                    Name = "Vegetables"
                },
            ];

            return categories;
        }
    }
}
