namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;

    public interface IPizzaRepository : IRepository<Pizza, int, PizzaRepository>
    {
        public Task<Pizza?> GetByIdWithIngredientsAsync(int id);

        public Task<ICollection<Pizza>> GetAllBasePizzasAsync();
    }
}
