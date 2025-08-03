namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;

    public interface IPizzaRepository : IRepository<Pizza, int, IPizzaRepository>
    {
        public Task<Pizza?> GetByIdWithIngredientsAsync(int id);

        public Task<ICollection<Pizza>> GetAllBasePizzasAsync();
        
        Task<ICollection<Pizza>> TakeBasePizzasWithIngredientsAsync(int take, int skip = 0);
    }
}
