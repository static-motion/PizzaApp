namespace PizzaApp.Data.Repository
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class ToppingRepository : BaseRepository<ToppingCategory, int>, IToppingRepository
    {
        public ToppingRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }
    }
}
