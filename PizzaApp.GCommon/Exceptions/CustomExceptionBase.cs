namespace PizzaApp.GCommon.Exceptions
{
    using System;

    public abstract class CustomExceptionBase : Exception
    {
        public CustomExceptionBase() { }

        protected CustomExceptionBase(string? message, params object[] parameters) 
            : base(FormatMessage(message, parameters))
        {
        }

        protected static string FormatMessage(string? message, params object[] parameters)
        {
            if (message is null)
                return string.Empty;
            

            if (parameters is null || parameters.Length == 0)
                return message;

            return string.Format(message, parameters);
        }
    }
}
