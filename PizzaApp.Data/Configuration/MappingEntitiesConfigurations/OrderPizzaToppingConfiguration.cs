namespace PizzaApp.Data.Configuration.MappingEntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models.MappingEntities;

    public class OrderPizzaToppingConfiguration : IEntityTypeConfiguration<OrderPizzaTopping>
    {
        public void Configure(EntityTypeBuilder<OrderPizzaTopping> entity)
        {
            entity
                .HasKey(e => new { e.OrderPizzaId, e.ToppingId });

            entity
                .Property(e => e.PriceAtPurchase)
                .HasColumnType("decimal(8,2)")
                .IsRequired();
        }
    }
}
