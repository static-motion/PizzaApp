namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;

    internal class ShoppingCartPizzaConfiguration : IEntityTypeConfiguration<ShoppingCartPizza>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartPizza> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .HasOne(e => e.BasePizza)
                .WithMany(p => p.ShoppingCarts)
                .HasForeignKey(e => e.BasePizzaId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            entity
                .HasOne(e => e.User)
                .WithMany(u => u.ShoppingCartPizzas)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            entity
                .Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasSentinel(0)
                .IsRequired();

            entity
                .Property(e => e.PizzaComponentsJson)
                .HasMaxLength(1000)
                .IsRequired(false);

            entity
                .Property(e => e.Price)
                .HasPrecision(8, 2)
                .HasColumnType("decimal(8,2)")
                .HasSentinel(0m)
                .IsRequired();
        }
    }
}