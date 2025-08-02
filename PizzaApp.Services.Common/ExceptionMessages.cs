namespace PizzaApp.Services.Common
{
    public static class ExceptionMessages
    {
        public const string UserNotFoundMessage = "Could not find user with ID: {0}";
        public const string AddressNotFoundMessage = "Could not find address with ID: {0}";
        public const string DrinkNotFoundMessage = "Could not find drink with ID: {0}";
        public const string DessertNotFoundMessage = "Could not find dessert with ID: {0}";
        public const string PizzaNotFoundMessage = "Could not find pizza with ID: {0}";
        public const string MenuCategoryNotImplementedMessage = "The category {0} is not currently implemented by this method.";
        public const string DeletedOrInactiveToppingMessage = "The topping with ID: {0} could not be found. It's deleted or otherwise inactive.";
    }
}
