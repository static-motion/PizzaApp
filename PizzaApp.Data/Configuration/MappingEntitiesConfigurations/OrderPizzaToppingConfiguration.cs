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
                .HasOne(e => e.OrderPizza)
                .WithMany(o => o.Toppings)
                .HasForeignKey(e => e.OrderPizzaId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            entity
                .HasOne(e => e.Topping)
                .WithMany(t => t.OrderPizzaToppings)
                .HasForeignKey(e => e.ToppingId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            entity
                .Property(e => e.PriceAtPurchase)
                .HasColumnType("decimal(8,2)")
                .IsRequired();
        }
    }
}
