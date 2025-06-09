using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaApp.Data.Models.MappingEntities;

namespace PizzaApp.Data.Configurations.MappingEntitiesConfigurations
{
    class OrderDessertConfiguration : IEntityTypeConfiguration<OrderDessert>
    {
        public void Configure(EntityTypeBuilder<OrderDessert> entity)
        {
            entity
                .HasKey(e => new { e.OrderId, e.DessertId });

            entity
                .HasOne(e => e.Order)
                .WithMany(o => o.OrderDeserts)
                .HasForeignKey(e => e.OrderId)
                .IsRequired();

            entity
                .HasOne(e => e.Dessert)
                .WithMany(d => d.Orders)
                .HasForeignKey(e => e.DessertId)
                .IsRequired();

            entity
                .Property(e => e.Quantity)
                .IsRequired()
                .HasSentinel(0);

            entity
                .Property(e => e.PricePerItemAtPurchase)
                .IsRequired()
                .HasColumnType("decimal(8,2)")
                .HasSentinel(0);
        }
    }
}
