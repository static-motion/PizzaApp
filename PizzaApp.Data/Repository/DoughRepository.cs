namespace PizzaApp.Data.Repository
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class DoughRepository : BaseRepository<Dough, int, IDoughRepository>, IDoughRepository
    {
        public DoughRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }
    }
}
