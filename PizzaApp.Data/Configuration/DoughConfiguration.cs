namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;
    using static PizzaApp.Data.Common.EntityConstraints.Dough;

    class DoughConfiguration : IEntityTypeConfiguration<Dough>
    {
        public void Configure(EntityTypeBuilder<Dough> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(TypeMaxLength);

            entity
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(e => e.Price)
                .HasPrecision(6, 2)
                .IsRequired();

            entity
                .HasData(CreateSeed());
        }

        private static List<Dough> CreateSeed()
        {
            List<Dough> doughs =
            [
                new Dough
                {
                    Id = 1,
                    Type = "White",
                    Description = "Our classic white dough recipe everyone knows and loves!",
                    Price = 7.5m
                },
                new Dough
                {
                    Id = 2,
                    Type = "Gluten-free",
                    Description = "Our special gluten-free dough!",
                    Price = 7.5m
                },
                new Dough
                {
                    Id = 3,
                    Type = "Wholegrain",
                    Description = "Our wholegrain dough packs additional fiber and protein for you fitness freaks out there!",
                    Price = 7.5m
                }
            ];

            return doughs;
        }
    }
}
