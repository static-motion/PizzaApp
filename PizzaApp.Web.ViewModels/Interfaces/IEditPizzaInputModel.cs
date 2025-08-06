namespace PizzaApp.Web.ViewModels.Interfaces
{
    public interface IEditPizzaInputModel : ICreatePizzaInputModel
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
