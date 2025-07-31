namespace PizzaApp.Data.Repository
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class DoughRepository : BaseRepository<Dough, int, DoughRepository>, IDoughRepository
    {
        public DoughRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }
    }
}
