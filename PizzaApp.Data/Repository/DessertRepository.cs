namespace PizzaApp.Data.Repository
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;

    public class DessertRepository : BaseRepository<Dessert, int, DessertRepository>, IDessertRepository
    {
        public DessertRepository(PizzaAppContext context)
            : base(context)
        {
        }
    }
}
