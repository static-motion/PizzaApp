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
                .HasKey(e => new { e.UserId, e.DessertId });

            entity
                .HasOne(e => e.User)
                .WithMany(sc => sc.ShoppingCartDesserts)
                .HasForeignKey(e => e.UserId)
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
