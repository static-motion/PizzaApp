namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Menu;
    using PizzaApp.Web.ViewModels.ShoppingCart;

    [TestFixture]
    public class CartServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IDrinkRepository> _drinkRepositoryMock;
        private Mock<IDessertRepository> _dessertRepositoryMock;
        private Mock<IPizzaCartService> _pizzaCartServiceMock;
        private CartService _cartService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _drinkRepositoryMock = new Mock<IDrinkRepository>();
            _dessertRepositoryMock = new Mock<IDessertRepository>();
            _pizzaCartServiceMock = new Mock<IPizzaCartService>();

            _cartService = new CartService(
                _userRepositoryMock.Object,
                _pizzaCartServiceMock.Object,
                _drinkRepositoryMock.Object,
                _dessertRepositoryMock.Object);
        }

        [TestCase(MenuCategory.Drinks)]
        [TestCase(MenuCategory.Desserts)]
        public async Task AddItemToCartAsync_WithValidItem_AddsItemToCart(MenuCategory category)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var menuItem = new MenuItemDetailsViewModel
            {
                Id = 1,
                Name = "test",
                Category = category,
                Quantity = 2
            };

            var user = new User
            {
                Id = userId,
                ShoppingCartDrinks = new List<ShoppingCartDrink>(),
                ShoppingCartDesserts = new List<ShoppingCartDessert>()
            };

            _userRepositoryMock.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            _drinkRepositoryMock.Setup(x => x.GetByIdAsync(menuItem.Id))
                .ReturnsAsync(new Drink { Id = menuItem.Id });

            _dessertRepositoryMock.Setup(x => x.GetByIdAsync(menuItem.Id))
                .ReturnsAsync(new Dessert { Id = menuItem.Id });

            // Act
            await _cartService.AddItemToCartAsync(menuItem, userId);

            // Assert
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            if (category == MenuCategory.Drinks)
            {
                Assert.That(user.ShoppingCartDrinks, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    Assert.That(user.ShoppingCartDrinks.First().DrinkId, Is.EqualTo(menuItem.Id));
                    Assert.That(user.ShoppingCartDrinks.First().Quantity, Is.EqualTo(menuItem.Quantity));
                });
            }
            else
            {
                Assert.That(user.ShoppingCartDesserts, Has.Count.EqualTo(1));
                Assert.Multiple(() =>
                {
                    Assert.That(user.ShoppingCartDesserts.First().DessertId, Is.EqualTo(menuItem.Id));
                    Assert.That(user.ShoppingCartDesserts.First().Quantity, Is.EqualTo(menuItem.Quantity));
                });
            }
            
        }

        [TestCase(MenuCategory.Drinks)]
        [TestCase(MenuCategory.Desserts)]
        public async Task AddItemToCartAsync_WithExistingItem_IncrementsQuantity(MenuCategory category)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var menuItem = new MenuItemDetailsViewModel
            {
                Id = 1,
                Name = "test",
                Category = category,
                Quantity = 2
            };

            var existingCartDrink = new ShoppingCartDrink
            {
                DrinkId = menuItem.Id,
                Quantity = 3,
                UserId = userId
            };

            var existingCartDessert = new ShoppingCartDessert
            {
                DessertId = menuItem.Id,
                Quantity = 3,
                UserId = userId
            };

            var user = new User
            {
                Id = userId,
                ShoppingCartDrinks = new List<ShoppingCartDrink> { existingCartDrink },
                ShoppingCartDesserts = new List<ShoppingCartDessert> { existingCartDessert }
            };

            _userRepositoryMock.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            _drinkRepositoryMock.Setup(x => x.GetByIdAsync(menuItem.Id))
                .ReturnsAsync(new Drink { Id = menuItem.Id });

            _dessertRepositoryMock.Setup(x => x.GetByIdAsync(menuItem.Id))
                .ReturnsAsync(new Dessert { Id = menuItem.Id });

            // Act
            await _cartService.AddItemToCartAsync(menuItem, userId);

            // Assert
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            if (category == MenuCategory.Drinks)
            {
                Assert.That(user.ShoppingCartDrinks, Has.Count.EqualTo(1));
                Assert.That(user.ShoppingCartDrinks.First().Quantity, Is.EqualTo(5));
            }
            else
            {
                Assert.That(user.ShoppingCartDesserts, Has.Count.EqualTo(1));
                Assert.That(user.ShoppingCartDesserts.First().Quantity, Is.EqualTo(5));
            }
        }

        [Test]
        public void AddItemToCartAsync_WithNonExistentUser_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var menuItem = new MenuItemDetailsViewModel
            {
                Id = 1,
                Name = "Coke",
                Category = MenuCategory.Drinks,
                Quantity = 1
            };

            _userRepositoryMock.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _cartService.AddItemToCartAsync(menuItem, userId));
        }

        [Test]
        public async Task AddPizzaToCartAsync_CallsPizzaCartService()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var pizzaDto = new PizzaCartDto();

            _pizzaCartServiceMock.Setup(x => x.AddPizzaToCartAsync(pizzaDto, userId))
                .Returns(Task.CompletedTask);

            // Act
            await _cartService.AddPizzaToCartAsync(pizzaDto, userId);

            // Assert
            _pizzaCartServiceMock.Verify(x => x.AddPizzaToCartAsync(pizzaDto, userId), Times.Once);
        }

        [Test]
        public async Task ClearShoppingCart_ClearsAllCartItems()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                ShoppingCartPizzas = new List<ShoppingCartPizza> { new ShoppingCartPizza() },
                ShoppingCartDrinks = new List<ShoppingCartDrink> { new ShoppingCartDrink() },
                ShoppingCartDesserts = new List<ShoppingCartDessert> { new ShoppingCartDessert() }
            };

            _userRepositoryMock.Setup(x => x.GetUserWithAddressesAndCartAsync(userId))
                .ReturnsAsync(user);

            // Act
            await _cartService.ClearShoppingCart(userId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(user.ShoppingCartPizzas, Is.Empty);
                Assert.That(user.ShoppingCartDrinks, Is.Empty);
                Assert.That(user.ShoppingCartDesserts, Is.Empty);
            });
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetUserCart_ReturnsCorrectCartViewWrapper()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                PhoneNumber = "1234567890",
                Addresses = new List<Address>
                {
                    new Address { Id = 1, City = "Test City", AddressLine1 = "Line 1", AddressLine2 = "Line 2" }
                },
                ShoppingCartDrinks = new List<ShoppingCartDrink>
                {
                    new ShoppingCartDrink
                    {
                        DrinkId = 1,
                        Quantity = 2,
                        Drink = new Drink { Id = 1, Name = "Cola", Price = 2.5m, ImageUrl = "cola.jpg" }
                    }
                },
                ShoppingCartDesserts = new List<ShoppingCartDessert>
                {
                    new ShoppingCartDessert
                    {
                        DessertId = 1,
                        Quantity = 1,
                        Dessert = new Dessert { Id = 1, Name = "Cake", Price = 3.5m, ImageUrl = "cake.jpg" }
                    }
                }
            };

            var pizzaViewModels = new List<CartPizzaViewModel>
            {
                new CartPizzaViewModel { Id = 1, Name = "Margherita", DoughName = "White", Quantity = 1, Price = 10.99m }
            };

            _userRepositoryMock.Setup(x => x.GetUserWithAddressesAndCartAsync(userId))
                .ReturnsAsync(user);

            _pizzaCartServiceMock.Setup(x => x.GetPizzasInCart(user))
                .ReturnsAsync(pizzaViewModels);

            // Act
            var result = await _cartService.GetUserCart(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.OrderDetails.PhoneNumber, Is.EqualTo(user.PhoneNumber));
                Assert.That(result.OrderDetails.AddressId, Is.EqualTo(user.Addresses.First().Id));

                Assert.That(result.Items.Pizzas.Count(), Is.EqualTo(1));
                Assert.That(result.Items.Drinks.Count(), Is.EqualTo(1));
                Assert.That(result.Items.Desserts.Count(), Is.EqualTo(1));

                Assert.That(result.Addresses.Count(), Is.EqualTo(1));
            });
        }

        [TestCase(MenuCategory.Drinks)]
        [TestCase(MenuCategory.Desserts)]
        [TestCase(MenuCategory.Pizzas)]
        public async Task RemoveItemFromCartAsync_WithDrink_RemovesItemFromCart(MenuCategory category)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var itemId = 1;
            var drink = new ShoppingCartDrink { DrinkId = itemId };
            var pizza = new ShoppingCartPizza { Id = itemId };
            var dessert = new ShoppingCartDessert { DessertId = itemId };

            var user = new User
            {
                Id = userId,
                ShoppingCartDrinks = new List<ShoppingCartDrink> { drink },
                ShoppingCartDesserts = new List<ShoppingCartDessert> { dessert },
                ShoppingCartPizzas = new List<ShoppingCartPizza> { pizza }
            };

            _userRepositoryMock.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            // Act
            await _cartService.RemoveItemFromCartAsync(itemId, userId, category);

            // Assert
            switch(category)
            {
                case MenuCategory.Drinks:
                    Assert.IsEmpty(user.ShoppingCartDrinks);
                    break;
                case MenuCategory.Pizzas:
                    Assert.IsEmpty(user.ShoppingCartPizzas);
                    break;
                case MenuCategory.Desserts:
                    Assert.IsEmpty(user.ShoppingCartDesserts);
                    break;

            }
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void RemoveItemFromCartAsync_WithNonExistentItem_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var itemId = 1;

            var user = new User
            {
                Id = userId,
                ShoppingCartDrinks = new List<ShoppingCartDrink>()
            };

            _userRepositoryMock.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _cartService.RemoveItemFromCartAsync(itemId, userId, MenuCategory.Drinks));
        }

        [Test]
        public void RemoveItemFromCartAsync_WithInvalidCategory_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var itemId = 1;

            var user = new User
            {
                Id = userId,
                ShoppingCartDrinks = new List<ShoppingCartDrink>()
            };

            _userRepositoryMock.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            // Act & Assert
            Assert.ThrowsAsync<MenuCategoryNotImplementedException>(() =>
                _cartService.RemoveItemFromCartAsync(itemId, userId, (MenuCategory)999));
        }
    }
}