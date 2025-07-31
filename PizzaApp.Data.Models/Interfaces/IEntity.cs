namespace PizzaApp.Data.Models.Interfaces
{
    public interface IEntity<TKey> where TKey : notnull
    {
        TKey Id { get; set; }
    }
}
