namespace PizzaApp.Services.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserSeedingService
    {
        Task SeedUsersAndRolesAsync();
    }
}