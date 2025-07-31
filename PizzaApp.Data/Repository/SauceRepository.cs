namespace PizzaApp.Data.Repository
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class SauceRepository : BaseRepository<Sauce, int, SauceRepository>, ISauceRepository
    {
        public SauceRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }
    }
}
