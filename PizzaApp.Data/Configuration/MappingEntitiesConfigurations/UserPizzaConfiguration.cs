﻿namespace PizzaApp.Data.Configuration.MappingEntitiesConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PizzaApp.Data.Models.MappingEntities;

    class UserPizzaConfiguration : IEntityTypeConfiguration<UserPizza>
    {
        public void Configure(EntityTypeBuilder<UserPizza> entity)
        {
            entity
                .HasKey(e => new { e.UserId, e.PizzaId });

            entity
                .HasOne(e => e.User)
                .WithMany(u => u.FavoritePizzas)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(e => e.Pizza)
                .WithMany(p => p.FavoritedBy)
                .HasForeignKey(e => e.PizzaId)
                .IsRequired();

            entity
                .HasQueryFilter(e => e.Pizza.IsDeleted == false);
        }
    }
}
