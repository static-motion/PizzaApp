namespace PizzaApp.Data.Configuration.MappingEntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models.MappingEntities;

    public class ShoppingCartDrinkConfiguration : IEntityTypeConfiguration<ShoppingCartDrink>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartDrink> entity)
        {
            entity
                .HasKey(e => new {e.ShoppingCartId, e.DrinkId});

            entity
                .HasOne(e => e.ShoppingCart)
                .WithMany(e => e.Drinks)
                .HasForeignKey(e => e.ShoppingCartId)
                .IsRequired();

            entity
                .HasOne(e => e.Drink)
                .WithMany(e => e.ShoppingCarts)
                .HasForeignKey(e => e.DrinkId)
                .IsRequired();

            entity
                .Property(e => e.Quantity)
                .IsRequired()
                .HasDefaultValue(1);
        }
    }
}
