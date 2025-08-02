namespace PizzaApp.Services.Common.Exceptions
{
    public class MenuCategoryNotImplementedException : CustomExceptionBase
    {
        public MenuCategoryNotImplementedException(string? message, params object[] parameters) : base(message, parameters)
        {
        }
    }
}
