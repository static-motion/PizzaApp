namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;
    using static PizzaApp.Data.Common.EntityConstraints.Address;

    class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .Property(e => e.City)
                .IsRequired()
                .HasMaxLength(CityMaxLength);

            entity
                .Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(AddressLineMaxLength);

            entity
                .Property(e => e.AddressLine2)
                .IsRequired(false)
                .HasMaxLength(AddressLineMaxLength);

            entity
                .HasOne(e => e.User)
                .WithMany(e => e.Addresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(e => e.IsDeleted)
                .IsRequired();
        }
    }
}
