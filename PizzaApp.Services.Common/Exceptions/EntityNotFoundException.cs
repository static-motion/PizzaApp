namespace PizzaApp.Services.Common.Exceptions
{
    using static PizzaApp.Services.Common.ExceptionMessages;
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string type, string id) 
            : base(string.Format(EntityNotFoundMessage, type, id))
        {
        }
    }
}
