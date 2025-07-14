namespace PizzaApp.Data.Repository
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class PizzaRepository : BaseRepository<Pizza, int>, IPizzaRepository
    {
        public PizzaRepository(PizzaAppContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
