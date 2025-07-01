using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.Data.Models.MappingEntities;

namespace PizzaApp.Data.Configurations.MappingEntitiesConfigurations
{
    class PizzaToppingConfiguration : IEntityTypeConfiguration<PizzaTopping>
    {
        public void Configure(EntityTypeBuilder<PizzaTopping> entity)
        {
            entity
                .HasKey(e => new { e.ToppingId, e.PizzaId });

            entity
                .HasOne(e => e.Pizza)
                .WithMany(p => p.Toppings)
                .HasForeignKey(e => e.PizzaId)
                .IsRequired();

            entity
                .HasOne(e => e.Topping)
                .WithMany(t => t.PizzasToppings)
                .HasForeignKey(e => e.ToppingId)
                .IsRequired();

            entity
                .HasData(
                    [
                        new PizzaTopping()
                        {
                            PizzaId = 1,
                            ToppingId = 1
                        },
                        new PizzaTopping()
                        {
                            PizzaId = 1,
                            ToppingId = 4
                        }
                    ]);
        }
    }
}
