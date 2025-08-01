namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Common.Serialization;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Orders;
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static PizzaApp.Services.Core.RepositoryHelper;
    using static PizzaApp.Services.Common.ExceptionMessages;

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDoughRepository _doughRepository;
        private readonly ISauceRepository _sauceRepository;
        private readonly IToppingCategoryRepository _toppingCategoryRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, 
            IDoughRepository doughRepository,
            ISauceRepository sauceRepository,
            IToppingCategoryRepository toppingCategoryRepository,
            IUserRepository userRepository)
        {
            this._orderRepository = orderRepository;
            this._doughRepository = doughRepository;
            this._sauceRepository = sauceRepository;
            this._toppingCategoryRepository = toppingCategoryRepository;
            this._userRepository = userRepository;
        }
        public async Task<IEnumerable<OrderViewWrapper>> GetOrdersAsync(Guid userId)
        {
            IEnumerable<Order> orders = await this._orderRepository.GetOrdersByUserIdAsync(userId);

            if (orders == null || !orders.Any())
            {
                return Enumerable.Empty<OrderViewWrapper>();
            }

            List<OrderViewWrapper> orderViewModels = new List<OrderViewWrapper>();

            IEnumerable<ToppingCategory> allToppingCategories = await this._toppingCategoryRepository
                .DisableTracking()
                .GetAllWithToppingsAsync();

            Dictionary<int, Dough> doughLookup = await GetEntityLookup(this._doughRepository);
            Dictionary<int, Sauce> sauceLookup = await GetEntityLookup(this._sauceRepository);

            foreach (Order order in orders)
            {
                OrderViewWrapper orderView = BuildOrderViewModel(allToppingCategories, doughLookup, sauceLookup, order);
                orderViewModels.Add(orderView);
            }

            return orderViewModels;
        }

        
        private static OrderViewWrapper BuildOrderViewModel(IEnumerable<ToppingCategory> allToppingCategories, Dictionary<int, Dough> doughLookup, Dictionary<int, Sauce> sauceLookup, Order order)
        {
            return new OrderViewWrapper
            {
                CreatedOn = order.CreatedOn,
                OrderStatus = order.OrderStatus,
                Pizzas = order.OrderPizzas.Select(op => new OrderPizzaViewModel
                {
                    Name = op.BasePizza.Name,
                    Quantity = op.Quantity,
                    Price = op.PricePerItemAtPurchase,
                    DoughName = doughLookup.TryGetValue(op.DoughId, out Dough? dough) ? dough.Type : "Unknown Dough",
                    SauceName = op.SauceId.HasValue && sauceLookup.TryGetValue(op.SauceId.Value, out Sauce? sauce) ? sauce.Type : "No Sauce",
                    Toppings = allToppingCategories.SelectMany(t => t.Toppings) // what a mess
                                        .Where(t => order.OrderPizzas.Any(op => op.Toppings.Any(opt => opt.ToppingId == t.Id)))
                                        .GroupBy(t => t.ToppingCategory.Name)
                                        .ToDictionary(
                                            g => g.Key,
                                            g => g.Select(t => t.Name).ToList()
                                        )
                }),
                Desserts = order.OrderDeserts.Select(od => new OrderDessertViewModel
                { 
                    Name = od.Dessert.Name,
                    Quantity = od.Quantity
                }),
                Drinks = order.OrderDrinks.Select(od => new OrderDrinkViewModel
                { 
                    Name = od.Drink.Name,
                    Quantity = od.Quantity
                })
            };
        }

        public async Task PlaceOrderAsync(OrderDetailsInputModel orderDetails, Guid userId)
        {
            User? user = await this._userRepository.GetUserWithShoppingCartAsync(userId)
                ?? throw new InvalidOperationException(UserNotFound); // TODO: Get user with addresses to validate
                                                                      // that the user is trying to order to an address they own

            if (user.ShoppingCartPizzas.Count == 0
                && user.ShoppingCartDrinks.Count == 0
                && user.ShoppingCartDesserts.Count == 0)
                return; // No items in cart to place an order


            IEnumerable<OrderDessert> orderDesserts = user.ShoppingCartDesserts
                .Select(d => new OrderDessert
                {
                    DessertId = d.DessertId,
                    Quantity = d.Quantity,
                    PricePerItemAtPurchase = d.Dessert.Price
                });

            IEnumerable<OrderDrink> orderDrinks = user.ShoppingCartDrinks
                .Select(d => new OrderDrink
                {
                    DrinkId = d.DrinkId,
                    Quantity = d.Quantity,
                    PricePerItemAtPurchase = d.Drink.Price
                });
            List<OrderPizza> orderPizzas = await this.CreateOrderPizzas(user);

            Order order = new()
            {
                UserId = user.Id,
                User = user,
                PhoneNumber = orderDetails.PhoneNumber, // TODO: handle phone number validation
                AddressId = orderDetails.AddressId,
                Comment = orderDetails.Comment,
                CreatedOn = DateTime.UtcNow,
                OrderStatus = OrderStatus.Received,
                OrderDeserts = user.ShoppingCartDesserts
                    .Select(d => new OrderDessert
                    {
                        DessertId = d.DessertId,
                        Quantity = d.Quantity,
                        PricePerItemAtPurchase = d.Dessert.Price
                    }).ToList(),
                OrderDrinks = user.ShoppingCartDrinks
                    .Select(d => new OrderDrink
                    {
                        DrinkId = d.DrinkId,
                        Quantity = d.Quantity,
                        PricePerItemAtPurchase = d.Drink.Price
                    }).ToList(),
                OrderPizzas = orderPizzas
            };

            order.Price = order.OrderDeserts.Sum(d => d.PricePerItemAtPurchase * d.Quantity)
                + order.OrderDrinks.Sum(d => d.PricePerItemAtPurchase * d.Quantity)
                + order.OrderPizzas.Sum(p => p.PricePerItemAtPurchase * p.Quantity);

            user.OrderHistory.Add(order);
            await this._userRepository.SaveChangesAsync();
        }

        private async Task<List<OrderPizza>> CreateOrderPizzas(User user)
        {
            List<OrderPizza> orderPizzas = new();
            IEnumerable<ToppingCategory> allToppingCategories = await this._toppingCategoryRepository
                .DisableTracking()
                .GetAllWithToppingsAsync();

            Dictionary<int, Dough> doughsLookup = await GetEntityLookup(this._doughRepository);
            Dictionary<int, Sauce> saucesLookup = await GetEntityLookup(this._sauceRepository);

            Dictionary<int, Topping> toppingsLookup = allToppingCategories
                .SelectMany(tc => tc.Toppings)
                .ToDictionary(t => t.Id);

            foreach (ShoppingCartPizza pizza in user.ShoppingCartPizzas)
            {
                PizzaComponentsDto components = pizza.GetComponentsFromJson()
                    ?? throw new InvalidOperationException("Pizza components not found or invalid."); //TODO: Handle

                OrderPizza orderPizza = new()
                {
                    BasePizzaId = pizza.BasePizzaId,
                    Quantity = pizza.Quantity,
                    DoughId = components.DoughId,
                    SauceId = components.SauceId,
                    Toppings = components.SelectedToppings
                        .Select(t => new OrderPizzaTopping
                        {
                            ToppingId = t,
                            PriceAtPurchase = toppingsLookup[t].Price
                        }).ToList()
                };

                decimal priceTotal = doughsLookup[components.DoughId].Price
                    + (components.SauceId.HasValue ? saucesLookup[components.SauceId.Value].Price : 0)
                    + components.SelectedToppings.Sum(t => toppingsLookup[t].Price);

                orderPizza.PricePerItemAtPurchase = priceTotal;

                orderPizzas.Add(orderPizza);
            }

            return orderPizzas;
        }

    }
}
