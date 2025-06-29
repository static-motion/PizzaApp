namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;

    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .HasOne(e => e.User)
                .WithMany(u => u.OrderHistory)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(e => e.OrderStatus)
                .IsRequired();

            entity
                .Property(e => e.Price)
                .HasPrecision(8, 2)
                .IsRequired();

            entity
                .Property(e => e.CreatedOn)
                .IsRequired();

            entity
                .HasOne(e => e.DeliveryAddress)
                .WithMany(a => a.Deliveries)
                .HasForeignKey(e => e.AddressId)
                .IsRequired();
        }
    }
}
