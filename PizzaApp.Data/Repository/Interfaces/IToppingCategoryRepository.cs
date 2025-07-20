namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;
    using System.Collections.Generic;

    public interface IToppingCategoryRepository : IRepository<ToppingCategory, int>
    {
        Task<IEnumerable<Topping>> GetAllToppingsFromRangeAsync(IEnumerable<int> selectedToppingIds);
        public Task<IEnumerable<ToppingCategory>> GetAllWithToppingsAsync(bool asNoTracking = false);
    }
}
