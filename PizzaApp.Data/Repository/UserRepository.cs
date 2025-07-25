﻿namespace PizzaApp.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using System.Threading.Tasks;

    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        public UserRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }

        public Task<User?> GetUserWithShoppingCartAsync(Guid userId)
        {
            return this.DbContext.Users
                .Include(c => c.ShoppingCartPizzas)
                    .ThenInclude(c => c.BasePizza)
                .Include(c => c.ShoppingCartDrinks)
                    .ThenInclude(c => c.Drink)
                .Include(c => c.ShoppingCartDesserts)
                    .ThenInclude(c => c.Dessert)
                .FirstOrDefaultAsync(c => c.Id == userId);
        }
    }
}
