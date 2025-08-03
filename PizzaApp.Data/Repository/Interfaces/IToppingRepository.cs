namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;

    public interface IToppingRepository : IRepository<Topping, int, IToppingRepository>
    {
        Task<IEnumerable<Topping>> GetAllToppingsFromRangeAsync(IEnumerable<int> selectedToppingIds);

        Task<IEnumerable<Topping>> TakeWithCategoriesAsync(int take, int skip = 0);
    }
}
