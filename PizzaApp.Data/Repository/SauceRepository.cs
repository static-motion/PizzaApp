namespace PizzaApp.Data.Repository
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class SauceRepository : BaseRepository<Sauce, int, ISauceRepository>, ISauceRepository
    {
        public SauceRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }
    }
}
