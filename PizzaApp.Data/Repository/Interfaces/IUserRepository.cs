namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;
    using System.Threading.Tasks;

    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetUserWithAddressesAsync(Guid userId);
        Task<User?> GetUserWithShoppingCartAsync(Guid userId);
        Task<User?> GetUserWithAddressesAndCartAsync(Guid userId);
    }
}
