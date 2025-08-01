namespace PizzaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models;

    using static PizzaApp.GCommon.EntityConstraints.Topping;

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
                .Property(e => e.Description)
                .HasMaxLength(DescriptionMaxLength)
                .IsRequired();

            entity
                .Property(e => e.Price)
                .HasPrecision(6, 2)
                .IsRequired();

            entity
                .HasData(GenerateToppingsSeed());

            entity
                .HasQueryFilter(e => e.IsDeleted == false
                    && e.ToppingCategory.IsDeleted == false);
        }

        private static IEnumerable<Topping> GenerateToppingsSeed()
        {
            return 
            [
                // Meats
                new Topping()
                {
                    Id = 1,
                    ToppingCategoryId = 1,
                    Name = "Pepperoni",
                    Description = "A spicy, cured Italian-American sausage with a bold, savory flavor and a slightly crispy texture when baked.",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 2,
                    ToppingCategoryId = 1,
                    Name = "Bacon",
                    Description = "Smoky, crispy and irresistibly delicious, bacon makes everything better - especially pizza!",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 3,
                    ToppingCategoryId = 1,
                    Name = "Chorizo",
                    Description = "Spicy, smoky Spanish sausage that kicks pizza up a notch - a flavor fiesta in every bite.",
                    Price = 1,
                },
                // Cheeses
                new Topping()
                {
                    Id = 4,
                    ToppingCategoryId = 2,
                    Name = "Mozzarella",
                    Description = "Creamy, melty, stretchy perfection. Pizza without mozzarella is just sad bread.",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 5,
                    ToppingCategoryId = 2,
                    Name = "Cheddar",
                    Description = "Sharp, tangy, and gloriously gooey. Cheddar brings a bold twist to pizza that basic cheeses can't match.",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 6,
                    ToppingCategoryId = 2,
                    Name = "Parmesan",
                    Description = "Salty, nutty, and irresistibly savory. Parmesan is the finishing touch that elevates pizza from good to gourmet.",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 7,
                    ToppingCategoryId = 2,
                    Name = "Philadelphia",
                    Description = "Velvety, indulgent, and irresistibly smooth. Philadelphia cheese turns pizza into a decadent delight.",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 8,
                    ToppingCategoryId = 3,
                    Name = "Bell Peppers",
                    Description = "Nature’s way of saying, ‘Yeah, this pizza needed more color.’",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 9,
                    ToppingCategoryId = 3,
                    Name = "Mushrooms",
                    Description = "The pizza topping that makes vegetarians and carnivores high-five!",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 10,
                    ToppingCategoryId = 3,
                    Name = "Onions",
                    Description = "Pizza’s way of keeping first dates interesting.",
                    Price = 1,
                },
                new Topping()
                {
                    Id = 11,
                    ToppingCategoryId = 3,
                    Name = "Olives",
                    Description = "Tiny, salty, and judging you for picking them off.",
                    Price = 1,
                },
            ];
        }
    }
}
