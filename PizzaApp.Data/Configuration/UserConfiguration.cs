namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;

    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity
                .HasOne(e => e.ShoppingCart)
                .WithOne(u => u.User)
                .HasForeignKey<User>(e => e.ShoppingCartId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
