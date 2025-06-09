//namespace PizzaApp.Data.Configuration
//{
//    using Microsoft.EntityFrameworkCore;
//    using Microsoft.EntityFrameworkCore.Metadata.Builders;
//    using PizzaApp.Data.Models;

//    using static PizzaApp.Data.Common.EntityConstraints.User;

//    class UserConfiguration : IEntityTypeConfiguration<User>
//    {
//        public void Configure(EntityTypeBuilder<User> entity)
//        {
//            entity
//                .HasKey(e => e.Id);

//            entity
//                .Property(e => e.Username)
//                .HasMaxLength(UsernameMaxLength)
//                .IsUnicode(false)
//                .IsRequired();

//            entity
//                .HasIndex(e => e.Username)
//                .IsUnique();

//            //entity
//            //    .HasData(CreateSeed());
//        }

//        private static List<User> CreateSeed()
//        {
//            List<User> users = new()
//            {
//                new User()
//                {
//                    Id = Guid.Parse("4b07d3b6-e467-444b-a5da-6f7052d3c5cb"),
//                    Username = "Admin",
//                },
//                new User() 
//                {
//                    Id = Guid.Parse("152453e9-1c0e-4e3f-bae2-280c648bf0b2"),
//                    Username = "Ghost"
//                }
//            };

//            return users;
//        }
//    }
//}
