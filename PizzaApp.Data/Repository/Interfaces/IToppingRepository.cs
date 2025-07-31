namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;

    public interface IToppingRepository : IRepository<Topping, int, ToppingRepository>
    {
        Task<IEnumerable<Topping>> GetAllToppingsFromRangeAsync(IEnumerable<int> selectedToppingIds);
    }
}
