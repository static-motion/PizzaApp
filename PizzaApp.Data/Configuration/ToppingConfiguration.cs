namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;

    using static PizzaApp.Data.Common.EntityConstraints.Topping;

    public class ToppingConfiguration : IEntityTypeConfiguration<Topping>
    {
        public void Configure(EntityTypeBuilder<Topping> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .HasOne(e => e.ToppingCategory)
                .WithMany(tt => tt.Toppings)
                .HasForeignKey(e => e.ToppingCategoryId)
                .IsRequired();

            entity
                .Property(e => e.Name)
                .HasMaxLength(NameMaxLength)
                .IsRequired();

            entity
                .Property(e => e.Desicription)
                .HasMaxLength(DescriptionMaxLength)
                .IsRequired();

            entity
                .Property(e => e.Price)
                .HasPrecision(6, 2)
                .IsRequired();
        }
    }
}
