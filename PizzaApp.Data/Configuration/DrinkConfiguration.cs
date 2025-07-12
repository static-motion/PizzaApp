namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;

    using static PizzaApp.Data.Common.EntityConstraints.Drink;

    class DrinkConfiguration : IEntityTypeConfiguration<Drink>
    {
        public void Configure(EntityTypeBuilder<Drink> entity)
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
                .IsRequired();

            entity
                .Property(e => e.Price)
                .HasPrecision(6, 2)
                .HasSentinel(0m)
                .IsRequired();

            entity
                .Property(e => e.ImageUrl)
                .HasMaxLength(ImageUrlMaxLength)
                .IsRequired(false);

            entity
                .HasData(CreateSeed());

            entity
                .HasQueryFilter(e => e.IsDeleted == false);
        }

        private static List<Drink> CreateSeed()
        {
            List<Drink> drinks =
            [
                new Drink
                {
                    Id = 1,
                    Name = "Coca Cola 500ml",
                    Description = "The refreshing original taste of Coca Cola!",
                    Price = 3m
                },
                new Drink 
                {
                    Id = 2,
                    Name = "Fanta 500ml",
                    Description = "The classic orange Fanta!",
                    Price = 3m,
                },
                new Drink
                {
                    Id = 3,
                    Name = "Sprite 500ml",
                    Description = "Who can say no to a pizza and sprite combo?",
                    Price = 3m
                },
                new Drink
                {
                    Id = 4,
                    Name = "Ayran 1L",
                    Description = "Salty and delicious - the perfect drink for a hearty lunch!",
                    Price = 2.5m
                },
                new Drink
                {
                    Id = 5,
                    Name = "Coca Cola Zero 500ml",
                    Description = "100% flavor, 0 calories! Sounds like a bargain to us!",
                    Price = 3m
                },
            ];

            return drinks;
        }
    }
}
