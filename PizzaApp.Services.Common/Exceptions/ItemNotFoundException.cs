namespace PizzaApp.Services.Common.Exceptions
{
    public class ItemNotFoundException : CustomExceptionBase
    {
        public ItemNotFoundException(string? message, params object[] parameters) : base(message, parameters)
        {
        }
    }
}
