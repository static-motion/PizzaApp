namespace PizzaApp.Services.Common
{
    public static class ExceptionMessages
    {
        public const string EntityNotFoundMessage = "Could not find {0} with ID: {1}";
        public const string MenuCategoryNotImplementedMessage = "The category {0} is not currently implemented by this method.";
        public const string DeletedOrInactiveToppingMessage = "The topping with ID: {0} could not be found. It's deleted or otherwise inactive.";
    }
}
