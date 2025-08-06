namespace PizzaApp.Services.Common.Exceptions
{
    using PizzaApp.GCommon.Exceptions;
    public class DeletedOrInactiveToppingException : CustomExceptionBase
    {
        public DeletedOrInactiveToppingException(string? message, params object[] parameters) 
            : base(message, parameters)
        {
        }
    }
}
