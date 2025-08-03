namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;

    public interface IDessertRepository :  IRepository<Dessert, int, IDessertRepository>//, DessertRepository>
    {
    }
}
