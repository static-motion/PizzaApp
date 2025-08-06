namespace PizzaApp.Services.Common.Exceptions
{
    using PizzaApp.GCommon.Exceptions;
    public class EntityNotFoundException : CustomExceptionBase
    {
        public EntityNotFoundException(string? message, params object[] parameters) : base(message, parameters)
        {
        }
    }
}
