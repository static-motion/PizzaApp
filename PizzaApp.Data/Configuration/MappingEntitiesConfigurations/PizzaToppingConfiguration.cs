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
                .Property(e => e.Quantity)
                .IsRequired()
                .HasSentinel(0);

            entity
                .Property(e => e.PricePerItemAtPurchase)
                .HasColumnType("decimal(8,2)")
                .IsRequired()
                .HasSentinel(0);
        }
    }
}
