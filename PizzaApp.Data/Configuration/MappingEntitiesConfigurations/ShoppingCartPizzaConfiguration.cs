namespace PizzaApp.Data.Configuration.MappingEntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models.MappingEntities;

    public class ShoppingCartPizzaConfiguration : IEntityTypeConfiguration<ShoppingCartPizza>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartPizza> entity)
        {
            entity
                .HasKey(e => new { e.ShoppingCartId, e.PizzaId });

            entity
                .HasOne(e => e.ShoppingCart)
                .WithMany(sc => sc.Pizzas)
                .HasForeignKey(e => e.ShoppingCartId)
                .IsRequired();

            entity
                .HasOne(e => e.Pizza)
                .WithMany(p => p.ShoppingCarts)
                .HasForeignKey(e => e.PizzaId)
                .IsRequired();

            entity
                .Property(e => e.Quantity)
                .IsRequired()
                .HasDefaultValue(1);
        }
    }
}
