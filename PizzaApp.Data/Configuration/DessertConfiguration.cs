namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;
    using static PizzaApp.Data.Common.EntityConstraints.Dessert;

    class DessertConfiguration : IEntityTypeConfiguration<Dessert>
    {
        public void Configure(EntityTypeBuilder<Dessert> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(e => e.Descripion)
                .IsRequired(false)
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(e => e.Price)
                .HasColumnType("decimal(8,2)")
                .HasSentinel(0m)
                .IsRequired();

            entity
                .Property(e => e.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(ImageUrlMaxLength);

            entity
                .HasData(CreateSeed());
        }

        private static List<Dessert> CreateSeed()
        {
            List<Dessert> desserts =
            [
                new Dessert
                {
                    Id = 1,
                    Name = "Cheesecake",
                    Descripion = "Rich cheesecake with blueberry jam! We could eat this all day!",
                    Price = 5m,
                },
                new Dessert
                {
                    Id = 2,
                    Name = "Chocolate Brownie",
                    Descripion = "Home baked with our special dough recipe and luxurious dark chocolate! Yum!",
                    Price = 5m,
                },
                new Dessert
                {
                    Id = 3,
                    Name = "Apple Pie",
                    Descripion = "Home is where the heart is. Or where the best apple pie is. We're still not sure...",
                    Price = 6m,
                },
                new Dessert
                {
                    Id = 4,
                    Name = "Vanilla Strawberry Ice Cream",
                    Descripion = "Vanilla Ice Cream. Strawberry syrup. Need we say more?",
                    Price = 5m,
                },
            ];
            return desserts;
        }
    }
}
