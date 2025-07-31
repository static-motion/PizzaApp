namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;
    using System.Collections.Generic;

    public interface IToppingCategoryRepository : IRepository<ToppingCategory, int, ToppingCategoryRepository>
    {
        
        public Task<IEnumerable<ToppingCategory>> GetAllWithToppingsAsync();
    }
}
