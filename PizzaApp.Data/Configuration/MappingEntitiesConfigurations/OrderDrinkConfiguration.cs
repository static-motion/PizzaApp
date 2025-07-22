namespace PizzaApp.Data.Configuration.MappingEntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models.MappingEntities;

    class OrderDrinkConfiguration : IEntityTypeConfiguration<OrderDrink>
    {
        public void Configure(EntityTypeBuilder<OrderDrink> entity)
        {
            entity.HasKey(e => new { e.OrderId, e.DrinkId });

            entity
                .HasOne(e => e.Order)
                .WithMany(o => o.OrderDrinks)
                .HasForeignKey(e => e.OrderId)
                .IsRequired();

            entity
                .HasOne(e => e.Drink)
                .WithMany(d => d.OrdersDrinks)
                .HasForeignKey(e => e.DrinkId)
                .IsRequired();

            entity
                .Property(e => e.Quantity)
                .HasSentinel(0)
                .IsRequired();

            entity
                .Property(e => e.PricePerItemAtPurchase)
                .HasColumnType("decimal(8,2)")
                .HasSentinel(0)
                .IsRequired();
        }
    }
}
