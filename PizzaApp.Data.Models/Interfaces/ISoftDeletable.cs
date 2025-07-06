namespace PizzaApp.Data.Models.Interfaces
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
