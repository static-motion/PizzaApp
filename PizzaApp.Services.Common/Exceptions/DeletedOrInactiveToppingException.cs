namespace PizzaApp.Services.Common.Exceptions
{
    public class DeletedOrInactiveToppingException : CustomExceptionBase
    {
        public DeletedOrInactiveToppingException(string? message, params object[] parameters) : base(message, parameters)
        {
        }
    }
}
