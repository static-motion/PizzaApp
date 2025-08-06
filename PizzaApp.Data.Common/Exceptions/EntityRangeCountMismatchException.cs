namespace PizzaApp.Data.Common.Exceptions
{
    using PizzaApp.GCommon.Exceptions;
    public class EntityRangeCountMismatchException : CustomExceptionBase
    {
        public EntityRangeCountMismatchException(string? message, params object[] parameters) 
            : base(message, parameters)
        {
        }
    }
}
