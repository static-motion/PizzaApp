namespace PizzaApp.GCommon.Enums
{
    public enum PizzaType
    {
        BasePizza = 1, // Base pizzas are the original pizzas that can be customized by users
        OrderPizza = 2, // Order pizzas are pizzas that have been ordered by users based on base pizzas with or without customizations to preserve data integrity
        CustomerPizza = 3, // Customer pizzas are pizzas that users have created themselves from scratch using the pizza creation form
    }
}
