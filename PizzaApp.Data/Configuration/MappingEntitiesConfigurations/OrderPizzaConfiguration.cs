namespace PizzaApp.Data.Configuration.MappingEntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models.MappingEntities;

    class OrderPizzaConfiguration : IEntityTypeConfiguration<OrderPizza>
    {
        public void Configure(EntityTypeBuilder<OrderPizza> entity)
        {
            entity.HasKey(e => new { e.OrderId, e.BasePizzaId });

            entity
                .HasOne(e => e.Order)
                .WithMany(o => o.OrderPizzas)
                .HasForeignKey(e => e.OrderId)
                .IsRequired();

            entity
                .HasOne(e => e.BasePizza)
                .WithMany(d => d.Orders)
                .HasForeignKey(e => e.BasePizzaId)
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


            entity
                .HasOne(e => e.Sauce)
                .WithMany(s => s.SauceOrders)
                .HasForeignKey(e => e.SauceId)
                .IsRequired(false);

            entity
                .HasOne(e => e.Dough)
                .WithMany(s => s.DoughOrders)
                .HasForeignKey(e => e.DoughId)
                .IsRequired(false);
        }
    }
}
