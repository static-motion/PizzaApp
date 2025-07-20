namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;

    public interface IPizzaRepository : IRepository<Pizza, int>
    {
        public Task<Pizza?> GetByIdWithIngredientsAsync(int id);
    }
}
