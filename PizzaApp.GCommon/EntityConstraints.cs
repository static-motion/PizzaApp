namespace PizzaApp.GCommon
{
    public static class EntityConstraints
    {
        public static class ToppingCategory
        {
            public const int NameMaxLength = 50;
            public const int NameMinLength = 2;
        }

        public static class Topping
        {
            public const int NameMaxLength = 50;
            public const int NameMinLength = 2;
            public const int DescriptionMaxLength = 150;
        }

        public static class Address
        {
            public const int CityMaxLength = 150;
            public const int CityMinLength = 2;
            public const int AddressLineMaxLength = 150;
            public const int AddressLineMinLength = 3;
        }

        public static class Dessert
        {
            public const int NameMaxLength = 50;
            public const int NameMinLength = 2;
            public const int DescriptionMaxLength = 150;
            public const int ImageUrlMaxLength = 1024;
        }

        public static class Dough
        {
            public const int TypeMinLength = 3;
            public const int TypeMaxLength = 50;
            public const int DescriptionMaxLength = 150;
            public const int DescriptionMinLength = 5;
        }

        public static class Drink
        {
            public const int NameMaxLength = 50;
            public const int NameMinLength = 2;
            public const int DescriptionMinLength = 3;
            public const int DescriptionMaxLength = 75;
            public const int ImageUrlMaxLength = 1024;
        }

        public static class Pizza
        {
            public const int NameMaxLength = 50;
            public const int NameMinLength = 2;
            public const int DescriptionMaxLength = 200;
            public const int ImageUrlMaxLength = 1024;
        }

        public static class Sauce 
        {
            public const int TypeMaxLength = 50;
            public const int TypeMinLength = 2;
            public const int DescriptionMaxLength = 150;
            public const int DescriptionMinLength = 5;
        }

        public static class User
        {
            public const int UsernameMaxLength = 20;
            public const int UsernameMinLength = 3;
        }
    }
}
