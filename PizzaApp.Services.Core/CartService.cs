namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Common.Serialization;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using PizzaApp.Web.ViewModels.Address;
    using PizzaApp.Web.ViewModels.Menu;
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using System.Collections.Generic;

    using static PizzaApp.Services.Common.ExceptionMessages;
    using static PizzaApp.Services.Core.LookupHelper;

    public class CartService : ICartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoughRepository _doughRepository;
        private readonly ISauceRepository _sauceRepository;
        private readonly IToppingCategoryRepository _toppingCategoryRepository;
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IDrinkRepository _drinkRepository;
        private readonly IDessertRepository _dessertRepository;

        public CartService(IUserRepository userRepository,
            IDoughRepository doughRepository, 
            ISauceRepository sauceRepository, 
            IToppingCategoryRepository toppingCategoryRepository,
            IPizzaRepository pizzaRepository,
            IDrinkRepository drinkRepository,
            IDessertRepository dessertRepository)
        {
            this._userRepository = userRepository;
            this._doughRepository = doughRepository;
            this._sauceRepository = sauceRepository;
            this._toppingCategoryRepository = toppingCategoryRepository;
            this._pizzaRepository = pizzaRepository;
            this._drinkRepository = drinkRepository;
            this._dessertRepository = dessertRepository;
        }

        public async Task<bool> AddItemToCartAsync(OrderItemViewModel orderItem, Guid userId)
        {
            User? user = await this._userRepository.GetUserWithShoppingCartAsync(userId);

            if (user is null)
            {
                return false;
            }

            bool result = orderItem.Category switch
            {
                MenuCategory.Drinks => await this.AddDrinkToCartAsync(orderItem, user),
                MenuCategory.Desserts => await this.AddDessertToCartAsync(orderItem, user),
                _ => false
            };

            return result;
        }

        private async Task<bool> AddDrinkToCartAsync(OrderItemViewModel item, User user)
        {
            Drink? drink = await this._drinkRepository.GetByIdAsync(item.Id);

            if (drink is null)
                return false;

            ShoppingCartDrink? cartDrink = user.ShoppingCartDrinks.FirstOrDefault(d => d.DrinkId == item.Id);
            if (cartDrink is not null)
            {
                cartDrink.Quantity += item.Quantity;
                return true;
            }

            user.ShoppingCartDrinks.Add(new ShoppingCartDrink()
            {
                DrinkId = item.Id,
                Quantity = item.Quantity,
                UserId = user.Id
            });

            await this._userRepository.SaveChangesAsync();
            return true;
        }

        private async Task<bool> AddDessertToCartAsync(OrderItemViewModel item, User user)
        {
            Dessert? dessert = await this._dessertRepository.GetByIdAsync(item.Id);

            if (dessert is null)
                return false;

            ShoppingCartDessert? cartDessert = user.ShoppingCartDesserts
                .FirstOrDefault(d => d.DessertId == item.Id);

            if (cartDessert is not null)
            {
                cartDessert.Quantity += item.Quantity;
                return true;
            }

            user.ShoppingCartDesserts.Add(new ShoppingCartDessert()
            {
                DessertId = item.Id,
                Quantity = item.Quantity,
                UserId = user.Id
            });

            await this._userRepository.SaveChangesAsync();
            return true;
        }

        // TODO: refactor later
        public async Task<bool> AddPizzaToCartAsync(PizzaCartDto pizzaDto, Guid userId)
        {
            User? user = await this._userRepository.GetUserWithShoppingCartAsync(userId);

            if (user is null)
            {
                return false;
            }

            PizzaComponentsDto components = new()
            {
                DoughId = pizzaDto.DoughId,
                SauceId = pizzaDto.SauceId,
                SelectedToppings = pizzaDto.SelectedToppingsIds
            };

            ShoppingCartPizza pizza = new()
            {
                BasePizzaId = pizzaDto.PizzaId,
                Quantity = pizzaDto.Quantity,
                // Price = 10m, // TODO: Calculate price based on components
                User = user,
                UserId = user.Id,
            };

            // serialize components to JSON
            pizza.SerializeComponentsToJson(components);

            // check if pizza already in cart
            ShoppingCartPizza? equalPizza = user.ShoppingCartPizzas.FirstOrDefault(p => 
                (p.GetComponentsFromJson()?.Equals(components) ?? false) // check if components match
                && pizza.BasePizzaId == p.BasePizzaId); // check if base pizza matches

            // if pizza with same components exists, increase quantity
            if (equalPizza is not null)
            {
                equalPizza.Quantity += pizzaDto.Quantity;
            }
            else // if not add new pizza to the cart
            {
                user.ShoppingCartPizzas.Add(pizza);
            }

            await this._userRepository.SaveChangesAsync();
            return true;
        }

        //TODO: The current cart index view doesn't use all properties from the view models. Fix when possible.
        public async Task<CartViewModel> GetUserCart(Guid userId)
        {
            User? user = await this._userRepository.GetUserWithAddressesAndCartAsync(userId)
                ?? throw new InvalidOperationException(UserNotFound);

            IEnumerable<CartPizzaViewModel> pizzasInCart = await this.GetAllPizzasInCart(user);
            IEnumerable<CartDrinkViewModel> drinksInCart = GetAllDrinksInCart(user);
            IEnumerable<CartDessertViewModel> dessertsInCart = GetAllDessertsInCart(user);

            CartItemsViewModel cartItems = new()
            {
                Pizzas = pizzasInCart,
                Drinks = drinksInCart,
                Desserts = dessertsInCart
            };
            IEnumerable<AddressViewModel> addresses = user.Addresses
                .Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    City = a.City,
                    AddressLine1 = a.AddressLine1,
                    AddressLine2 = a.AddressLine2,
                });

            CartViewModel shoppingCart = new()
            {
                Items = cartItems,
                Addresses = addresses,
                OrderDetails = new OrderDetailsInputModel()
                {
                    PhoneNumber = user.PhoneNumber
                }
            };

            return shoppingCart;
        }

        private static List<CartDessertViewModel> GetAllDessertsInCart(User user)
        {
            return user.ShoppingCartDesserts
                .Select(d => new CartDessertViewModel
                {
                    Id = d.DessertId,
                    Name = d.Dessert.Name,
                    Quantity = d.Quantity,
                    Price = d.Dessert.Price,
                    ImageUrl = d.Dessert.ImageUrl
                }).ToList();
        }

        private static IEnumerable<CartDrinkViewModel> GetAllDrinksInCart(User user)
        {
            return user.ShoppingCartDrinks
                .Select(d => new CartDrinkViewModel
                {
                    Id = d.DrinkId,
                    Name = d.Drink.Name,
                    Quantity = d.Quantity,
                    Price = d.Drink.Price,
                    ImageUrl = d.Drink.ImageUrl
                }).ToList();
        }

        private async Task<IEnumerable<CartPizzaViewModel>> GetAllPizzasInCart(User user)
        {
            IEnumerable<ToppingCategory> allToppingCategories = await this._toppingCategoryRepository
                .DisableTracking()
                .GetAllWithToppingsAsync();

            Dictionary<int, Dough> doughsById = await GetEntityLookup(this._doughRepository);
            Dictionary<int, Sauce> saucesLookup = await GetEntityLookup(this._sauceRepository);

            List<CartPizzaViewModel> pizzasInCart = new();

            foreach (ShoppingCartPizza pizza in user.ShoppingCartPizzas)
            {
                // load base pizza
                Pizza? basePizza = await this._pizzaRepository.GetByIdAsync(pizza.BasePizzaId);

                PizzaComponentsDto components =
                    pizza.GetComponentsFromJson()
                    ?? throw new InvalidOperationException("Pizza components not found or invalid.");

                if (basePizza is null)
                {
                    throw new InvalidOperationException($"Base pizza with ID {pizza.BasePizzaId} not found.");
                }
                // load dough and sauce
                Dough dough = doughsById[components.DoughId]; //TODO:

                Sauce? sauce = components.SauceId.HasValue ?
                    saucesLookup[components.SauceId.Value]
                    : null;

                // this is stupid I hate the repository pattern
                CartPizzaViewModel pizzaViewModel = new()
                {
                    Id = pizza.Id,
                    Name = basePizza.Name,
                    DoughName = dough?.Type ?? "Unknown Dough",
                    SauceName = sauce?.Type,
                    Quantity = pizza.Quantity
                };

                foreach (int toppingId in components.SelectedToppings)
                {
                    Topping? topping = allToppingCategories.SelectMany(tc => tc.Toppings)
                        .FirstOrDefault(t => t.Id == toppingId);
                    if (topping is null)
                    {
                        continue; // TODO: handle missing topping
                    }

                    string categoryName = topping.ToppingCategory.Name;

                    if (!pizzaViewModel.Toppings.TryGetValue(categoryName, out List<ToppingViewModel>? categoryItems))
                    {
                        categoryItems = new List<ToppingViewModel>();
                        pizzaViewModel.Toppings.Add(categoryName, categoryItems);
                    }

                    categoryItems.Add(new ToppingViewModel()
                    {
                        Id = topping.Id,
                        Name = topping.Name,
                        Price = topping.Price
                    });
                }

                pizzaViewModel.Price = pizzaViewModel.Toppings.Values
                    .SelectMany(t => t)
                    .Sum(t => t.Price) + dough.Price + sauce.Price;

                pizzasInCart.Add(pizzaViewModel);

            }
            return pizzasInCart;
        }

        public async Task<bool> RemoveItemFromCartAsync(int itemId, Guid userId, MenuCategory menuCategory)
        {
            User? user = await this._userRepository.GetUserWithShoppingCartAsync(userId);

            if (user is null)
                return false; // TODO: throw exception instead for better error handling

            switch (menuCategory)
            {
                case MenuCategory.Pizzas:
                    ShoppingCartPizza? pizzaToRemove = user.ShoppingCartPizzas.FirstOrDefault(p => p.Id == itemId);
                    if (pizzaToRemove is null)
                        return false;
                    user.ShoppingCartPizzas.Remove(pizzaToRemove);
                    break;
                case MenuCategory.Drinks:
                    ShoppingCartDrink? drinkToRemove = user.ShoppingCartDrinks.FirstOrDefault(d => d.DrinkId == itemId);
                    if (drinkToRemove is null)
                        return false;
                    user.ShoppingCartDrinks.Remove(drinkToRemove);
                    break;
                case MenuCategory.Desserts:
                    ShoppingCartDessert? dessertToRemove = user.ShoppingCartDesserts.FirstOrDefault(d => d.DessertId == itemId);
                    if (dessertToRemove is null)
                        return false;
                    user.ShoppingCartDesserts.Remove(dessertToRemove);
                    break;
                default:
                    return false; // Unsupported category TODO: Throw exception instead for better error handling
            }

            await this._userRepository.SaveChangesAsync();
            return true; // Successfully removed the item from the cart
        }

        public async Task ClearShoppingCart(Guid userId)
        {
            User? user = await this._userRepository.GetUserWithAddressesAndCartAsync(userId)
                ?? throw new InvalidOperationException(UserNotFound);
            user.ShoppingCartPizzas.Clear();
            user.ShoppingCartDrinks.Clear();
            user.ShoppingCartDesserts.Clear();
            await this._userRepository.SaveChangesAsync();
        }

    }
}
