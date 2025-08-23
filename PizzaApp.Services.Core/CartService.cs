namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Address;
    using PizzaApp.Web.ViewModels.Cart;
    using PizzaApp.Web.ViewModels.Menu;
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using System;
    using System.Threading.Tasks;

    public class CartService : ICartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDrinkRepository _drinkRepository;
        private readonly IDessertRepository _dessertRepository;
        private readonly IPizzaCartService _pizzaCartService;

        public CartService(
            IUserRepository userRepository,
            IPizzaCartService pizzaCartService,
            IDrinkRepository drinkRepository,
            IDessertRepository dessertRepository)
        {
            _userRepository = userRepository;
            _pizzaCartService = pizzaCartService;
            _drinkRepository = drinkRepository;
            _dessertRepository = dessertRepository;
        }

        public async Task AddItemToCartAsync(MenuItemDetailsViewModel orderItem, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserWithShoppingCartAsync(userId)
                    ?? throw new EntityNotFoundException(nameof(User), userId.ToString());

                switch (orderItem.Category)
                {
                    case MenuCategory.Drinks:
                        await this.AddDrinkToCartAsync(orderItem, user);
                        break;
                    case MenuCategory.Desserts:
                        await this.AddDessertToCartAsync(orderItem, user);
                        break;
                    default:
                        throw new MenuCategoryNotImplementedException(orderItem.Category.ToString());
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task AddDrinkToCartAsync(MenuItemDetailsViewModel item, User user)
        {
            Drink? drink = await this._drinkRepository.GetByIdAsync(item.Id)
                ?? throw new EntityNotFoundException(nameof(Drink), item.Id.ToString());
            
            ShoppingCartDrink? cartDrink = user.ShoppingCartDrinks
                .FirstOrDefault(d => d.DrinkId == item.Id);

            if (cartDrink is not null)
            {
                cartDrink.Quantity += item.Quantity;
            } 
            else
            {
                user.ShoppingCartDrinks.Add(new ShoppingCartDrink()
                {
                    DrinkId = item.Id,
                    Quantity = item.Quantity,
                    UserId = user.Id
                });
            }

            await this._userRepository.SaveChangesAsync();
        }

        private async Task AddDessertToCartAsync(MenuItemDetailsViewModel item, User user)
        {
            Dessert? dessert = await this._dessertRepository.GetByIdAsync(item.Id)
                ?? throw new EntityNotFoundException(nameof(Dessert), item.Id.ToString());

            ShoppingCartDessert? cartDessert = user.ShoppingCartDesserts
                .FirstOrDefault(d => d.DessertId == item.Id);

            if (cartDessert is not null)
            {
                cartDessert.Quantity += item.Quantity;
            }
            else
            {
                user.ShoppingCartDesserts.Add(new ShoppingCartDessert()
                {
                    DessertId = item.Id,
                    Quantity = item.Quantity,
                    UserId = user.Id
                });
            }

            await this._userRepository.SaveChangesAsync();
        }

        public Task AddPizzaToCartAsync(PizzaCartDto pizzaDto, Guid userId)
        {
            return this._pizzaCartService.AddPizzaToCartAsync(pizzaDto, userId);
        }

        public async Task ClearShoppingCart(Guid userId)
        {
            User? user = await this._userRepository.GetUserWithAddressesAndCartAsync(userId)
                ?? throw new EntityNotFoundException(nameof(User), userId.ToString());

            user.ShoppingCartPizzas.Clear();
            user.ShoppingCartDrinks.Clear();
            user.ShoppingCartDesserts.Clear();

            await this._userRepository.SaveChangesAsync();
        }

        public async Task<CartViewWrapper> GetUserCart(Guid userId)
        {
            User? user = await _userRepository.GetUserWithAddressesAndCartAsync(userId)
            ?? throw new EntityNotFoundException(nameof(User), userId.ToString());

            return new CartViewWrapper
            {
                Items = new CartItemsViewWrapper
                {
                    Pizzas = await _pizzaCartService.GetPizzasInCart(user),
                    Drinks = GetDrinksInCart(user),
                    Desserts = GetDessertsInCart(user)
                },
                Addresses = GetUserAddresses(user),
                OrderDetails = new OrderDetailsInputModel
                {
                    PhoneNumber = user.PhoneNumber ?? "",
                    AddressId = user.Addresses.FirstOrDefault()?.Id
                }
            };
        }

        private static IEnumerable<AddressViewModel> GetUserAddresses(User user)
        {
            return user.Addresses
                .Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    City = a.City,
                    AddressLine1 = a.AddressLine1,
                    AddressLine2 = a.AddressLine2,
                });
        }

        private static List<CartDessertViewModel> GetDessertsInCart(User user)
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

        private static List<CartDrinkViewModel> GetDrinksInCart(User user)
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

        public async Task RemoveItemFromCartAsync(int itemId, Guid userId, MenuCategory menuCategory)
        {
            User? user = await this._userRepository.GetUserWithShoppingCartAsync(userId)
                ?? throw new EntityNotFoundException(nameof(User), userId.ToString());
            
            switch (menuCategory)
            {
                case MenuCategory.Pizzas:
                    ShoppingCartPizza? pizzaToRemove = user.ShoppingCartPizzas.FirstOrDefault(p => p.Id == itemId) 
                        ?? throw new EntityNotFoundException(nameof(ShoppingCartPizza), itemId.ToString());
                    user.ShoppingCartPizzas.Remove(pizzaToRemove);
                    break;
                case MenuCategory.Drinks:
                    ShoppingCartDrink? drinkToRemove = user.ShoppingCartDrinks.FirstOrDefault(d => d.DrinkId == itemId)
                        ?? throw new EntityNotFoundException(nameof(ShoppingCartDrink), itemId.ToString());
                    user.ShoppingCartDrinks.Remove(drinkToRemove);
                    break;
                case MenuCategory.Desserts:
                    ShoppingCartDessert? dessertToRemove = user.ShoppingCartDesserts.FirstOrDefault(d => d.DessertId == itemId)
                        ?? throw new EntityNotFoundException(nameof(ShoppingCartDessert), itemId.ToString());
                    user.ShoppingCartDesserts.Remove(dessertToRemove);
                    break;
                default:
                    throw new MenuCategoryNotImplementedException
                        (menuCategory.ToString());
            }

            await this._userRepository.SaveChangesAsync();
        }
    }
}
