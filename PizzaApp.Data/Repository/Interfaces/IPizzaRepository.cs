namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;

    public interface IPizzaRepository : IRepository<Pizza, int, IPizzaRepository>
    {
        public Task<ICollection<Pizza>> GetAllBasePizzasAsync();

        public Task<ICollection<Pizza>> GetAllUserPizzasAsync(Guid userId);

        public Task<Pizza?> GetByIdWithIngredientsAsync(int id);

        Task<IEnumerable<Pizza>> TakeBasePizzasWithIngredientsAsync(int take, int skip = 0);
    }
}
