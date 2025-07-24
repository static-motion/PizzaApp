namespace PizzaApp.GCommon.Extensions
{
    using PizzaApp.GCommon.Enums;

    public static class MenuCategoryExtensions
    {
        public static MenuCategory? FromString(string categoryString)
        {
            if (string.IsNullOrEmpty(categoryString))
                return null;

            return Enum.TryParse(categoryString, true, out MenuCategory result)
                ? result
                : null;
        }
    }
}
