namespace PizzaApp.Data.Configuration.MappingEntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models.MappingEntities;

    public class ShoppingCartDessertConfiguration : IEntityTypeConfiguration<ShoppingCartDessert>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartDessert> entity)
        {
            entity
                .HasKey(e => new { e.ShoppingCartId, e.DessertId });

            entity
                .HasOne(e => e.ShoppingCart)
                .WithMany(sc => sc.Desserts)
                .HasForeignKey(e => e.ShoppingCartId)
                .IsRequired();
            
            entity
                .HasOne(e => e.Dessert)
                .WithMany(e => e.ShoppingCarts)
                .HasForeignKey(e => e.DessertId)
                .IsRequired();

            entity
                .Property(e => e.Quantity)
                .IsRequired()
                .HasDefaultValue(1);
        }
    }
}
