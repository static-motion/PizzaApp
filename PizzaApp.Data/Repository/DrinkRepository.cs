namespace PizzaApp.Data.Repository
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class DrinkRepository : BaseRepository<Drink, int, DrinkRepository>, IDrinkRepository
    {
        public DrinkRepository(PizzaAppContext dbContext) : base(dbContext)
        {
        }
    }
}
