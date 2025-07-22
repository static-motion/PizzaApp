namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;
    using System.Threading.Tasks;

    public interface ICartRepository : IRepository<ShoppingCart, int>
    {
        Task<ShoppingCart?> GetCompleteCartByUserIdAsync(Guid userId);
    }
}
