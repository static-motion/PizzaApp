namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using PizzaApp.Data.Common.Serialization;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core;
    using PizzaApp.Web.ViewModels.ShoppingCart;

    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IDoughRepository> _mockDoughRepository;
        private Mock<ISauceRepository> _mockSauceRepository;
        private Mock<IToppingCategoryRepository> _mockToppingCategoryRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockDoughRepository = new Mock<IDoughRepository>();
            _mockSauceRepository = new Mock<ISauceRepository>();
            _mockToppingCategoryRepository = new Mock<IToppingCategoryRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _orderService = new OrderService(
                _mockOrderRepository.Object,
                _mockDoughRepository.Object,
                _mockSauceRepository.Object,
                _mockToppingCategoryRepository.Object,
                _mockUserRepository.Object
            );
        }

        #region GetOrdersAsync Tests

        [Test]
        public async Task GetOrdersAsync_WithValidUserId_ReturnsOrderViewWrappers()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orders = CreateSampleOrders();
            var toppingCategories = CreateSampleToppingCategories();
            var doughLookup = CreateSampleDoughLookup();
            var sauceLookup = CreateSampleSauceLookup();

            SetupRepositoryMocks(orders, toppingCategories, doughLookup, sauceLookup);

            // Act
            var result = await _orderService.GetOrdersAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));

            var firstOrder = result.First();
            Assert.That(firstOrder.OrderStatus, Is.EqualTo(OrderStatus.Received));
            Assert.That(firstOrder.Pizzas.Any(), Is.True);
        }

        [Test]
        public async Task GetOrdersAsync_WithNoOrders_ReturnsEmptyCollection()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var emptyOrders = Enumerable.Empty<Order>();

            _mockOrderRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockOrderRepository.Object);
            _mockOrderRepository.Setup(x => x.DisableTracking()).Returns(_mockOrderRepository.Object);
            _mockOrderRepository.Setup(x => x.GetOrdersByUserIdAsync(userId))
                .ReturnsAsync(emptyOrders);

            // Act
            var result = await _orderService.GetOrdersAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetOrdersAsync_WithNullOrders_ReturnsEmptyCollection()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockOrderRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockOrderRepository.Object);
            _mockOrderRepository.Setup(x => x.DisableTracking()).Returns(_mockOrderRepository.Object);
            _mockOrderRepository.Setup(x => x.GetOrdersByUserIdAsync(userId))
                .ReturnsAsync((IEnumerable<Order>)null);

            // Act
            var result = await _orderService.GetOrdersAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        #endregion

        #region PlaceOrderAsync Tests

        [Test]
        public async Task PlaceOrderAsync_WithValidOrderAndItems_PlacesOrderSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orderDetails = CreateValidOrderDetails();
            var user = CreateUserWithShoppingCartItems();

            var toppingCategories = CreateSampleToppingCategories();
            var doughLookup = CreateSampleDoughLookup();
            var sauceLookup = CreateSampleSauceLookup();

            _mockUserRepository.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            SetupToppingRepositoryMocks(toppingCategories, doughLookup, sauceLookup);

            // Act
            await _orderService.PlaceOrderAsync(orderDetails, userId);

            // Assert
            _mockUserRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.That(user.OrderHistory.Count, Is.EqualTo(1));

            var placedOrder = user.OrderHistory.First();
            Assert.That(placedOrder.OrderStatus, Is.EqualTo(OrderStatus.Received));
            Assert.That(placedOrder.PhoneNumber, Is.EqualTo(orderDetails.PhoneNumber));
            Assert.That(placedOrder.AddressId, Is.EqualTo(orderDetails.AddressId));
            Assert.That(placedOrder.Price, Is.GreaterThan(0));
        }

        [Test]
        public void PlaceOrderAsync_WithNonExistentUser_ThrowsEntityNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orderDetails = CreateValidOrderDetails();

            _mockUserRepository.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => _orderService.PlaceOrderAsync(orderDetails, userId));
        }

        [Test]
        public async Task PlaceOrderAsync_WithEmptyShoppingCart_DoesNotCreateOrder()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orderDetails = CreateValidOrderDetails();
            var userWithEmptyCart = CreateUserWithEmptyShoppingCart();

            _mockUserRepository.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(userWithEmptyCart);

            // Act
            await _orderService.PlaceOrderAsync(orderDetails, userId);

            // Assert
            _mockUserRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
            Assert.That(userWithEmptyCart.OrderHistory.Count, Is.EqualTo(0));
        }

        #endregion

        #region Helper Methods

        private void SetupRepositoryMocks(IEnumerable<Order> orders, IEnumerable<ToppingCategory> toppingCategories,
            Dictionary<int, Dough> doughLookup, Dictionary<int, Sauce> sauceLookup)
        {
            _mockOrderRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockOrderRepository.Object);
            _mockOrderRepository.Setup(x => x.DisableTracking()).Returns(_mockOrderRepository.Object);
            _mockOrderRepository.Setup(x => x.GetOrdersByUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(orders);

            _mockToppingCategoryRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockToppingCategoryRepository.Object);
            _mockToppingCategoryRepository.Setup(x => x.DisableTracking()).Returns(_mockToppingCategoryRepository.Object);
            _mockToppingCategoryRepository.Setup(x => x.GetAllCategoriesWithToppingsAsync())
                .ReturnsAsync(toppingCategories);

            _mockDoughRepository.Setup(x => x.DisableTracking()).Returns(_mockDoughRepository.Object);
            _mockDoughRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockDoughRepository.Object);
            _mockDoughRepository.Setup(x => x.GetLookup()).ReturnsAsync(doughLookup);

            _mockSauceRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockSauceRepository.Object);
            _mockSauceRepository.Setup(x => x.DisableTracking()).Returns(_mockSauceRepository.Object);
            _mockSauceRepository.Setup(x => x.GetLookup()).ReturnsAsync(sauceLookup);
        }

        private void SetupToppingRepositoryMocks(IEnumerable<ToppingCategory> toppingCategories,
            Dictionary<int, Dough> doughLookup, Dictionary<int, Sauce> sauceLookup)
        {
            _mockToppingCategoryRepository.Setup(x => x.DisableTracking()).Returns(_mockToppingCategoryRepository.Object);
            _mockToppingCategoryRepository.Setup(x => x.GetAllCategoriesWithToppingsAsync())
                .ReturnsAsync(toppingCategories);

            _mockDoughRepository.Setup(x => x.DisableTracking()).Returns(_mockDoughRepository.Object);
            _mockDoughRepository.Setup(x => x.GetLookup()).ReturnsAsync(doughLookup);

            _mockSauceRepository.Setup(x => x.DisableTracking()).Returns(_mockSauceRepository.Object);
            _mockSauceRepository.Setup(x => x.GetLookup()).ReturnsAsync(sauceLookup);
        }

        private IEnumerable<Order> CreateSampleOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    OrderStatus = OrderStatus.Received,
                    OrderPizzas = new List<OrderPizza>
                    {
                        new OrderPizza
                        {
                            BasePizza = new Pizza { Id = 1, Name = "Margherita" },
                            Quantity = 1,
                            PricePerItemAtPurchase = 12.99m,
                            DoughId = 1,
                            SauceId = 1,
                            Toppings = new List<OrderPizzaTopping>
                            {
                                new OrderPizzaTopping { ToppingId = 1 }
                            }
                        }
                    },
                    OrderDeserts = new List<OrderDessert>
                    {
                        new OrderDessert
                        {
                            Dessert = new Dessert { Name = "Tiramisu" },
                            Quantity = 1
                        }
                    },
                    OrderDrinks = new List<OrderDrink>
                    {
                        new OrderDrink
                        {
                            Drink = new Drink { Name = "Coca Cola" },
                            Quantity = 2
                        }
                    }
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    OrderStatus = OrderStatus.Delivered,
                    OrderPizzas = new List<OrderPizza>(),
                    OrderDeserts = new List<OrderDessert>(),
                    OrderDrinks = new List<OrderDrink>()
                }
            };
        }

        private IEnumerable<ToppingCategory> CreateSampleToppingCategories()
        {
            return new List<ToppingCategory>
            {
                new ToppingCategory
                {
                    Id = 1,
                    Name = "Cheese",
                    Toppings = new List<Topping>
                    {
                        new Topping
                        {
                            Id = 1,
                            Name = "Mozzarella",
                            Price = 1.50m,
                            ToppingCategory = new ToppingCategory { Name = "Cheese" }
                        }
                    }
                },
                new ToppingCategory
                {
                    Id = 2,
                    Name = "Meat",
                    Toppings = new List<Topping>
                    {
                        new Topping
                        {
                            Id = 2,
                            Name = "Pepperoni",
                            Price = 2.00m,
                            ToppingCategory = new ToppingCategory { Name = "Meat" }
                        }
                    }
                }
            };
        }

        private Dictionary<int, Dough> CreateSampleDoughLookup()
        {
            return new Dictionary<int, Dough>
            {
                { 1, new Dough { Id = 1, Type = "Thin Crust", Price = 2.00m } },
                { 2, new Dough { Id = 2, Type = "Thick Crust", Price = 2.50m } }
            };
        }

        private Dictionary<int, Sauce> CreateSampleSauceLookup()
        {
            return new Dictionary<int, Sauce>
            {
                { 1, new Sauce { Id = 1, Type = "Tomato", Price = 1.00m } },
                { 2, new Sauce { Id = 2, Type = "BBQ", Price = 1.50m } }
            };
        }

        private OrderDetailsInputModel CreateValidOrderDetails()
        {
            return new OrderDetailsInputModel
            {
                PhoneNumber = "+1234567890",
                AddressId = 1,
                Comment = "Test order comment"
            };
        }

        private User CreateUserWithShoppingCartItems()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                OrderHistory = new List<Order>(),
                ShoppingCartPizzas = new List<ShoppingCartPizza>
                {
                    CreateValidShoppingCartPizza()
                },
                ShoppingCartDesserts = new List<ShoppingCartDessert>
                {
                    new ShoppingCartDessert
                    {
                        DessertId = 1,
                        Quantity = 1,
                        Dessert = new Dessert { Price = 4.99m }
                    }
                },
                ShoppingCartDrinks = new List<ShoppingCartDrink>
                {
                    new ShoppingCartDrink
                    {
                        DrinkId = 1,
                        Quantity = 2,
                        Drink = new Drink { Price = 1.99m }
                    }
                }
            };

            return user;
        }

        private User CreateUserWithEmptyShoppingCart()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                OrderHistory = new List<Order>(),
                ShoppingCartPizzas = new List<ShoppingCartPizza>(),
                ShoppingCartDesserts = new List<ShoppingCartDessert>(),
                ShoppingCartDrinks = new List<ShoppingCartDrink>()
            };
        }

        private User CreateUserWithInvalidPizzas()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                OrderHistory = new List<Order>(),
                ShoppingCartPizzas = new List<ShoppingCartPizza>
                {
                    CreateInvalidShoppingCartPizza()
                },
                ShoppingCartDesserts = new List<ShoppingCartDessert>(),
                ShoppingCartDrinks = new List<ShoppingCartDrink>()
            };

            return user;
        }

        private ShoppingCartPizza CreateValidShoppingCartPizza()
        {
            var pizza = new Mock<ShoppingCartPizza>();
            pizza.SetupGet(p => p.BasePizzaId).Returns(1);
            pizza.SetupGet(p => p.Quantity).Returns(1);

            // Mock the GetComponentsFromJson method
            pizza.Setup(p => p.GetComponentsFromJson())
                .Returns(new PizzaComponentsDto
                {
                    DoughId = 1,
                    SauceId = 1,
                    SelectedToppings = new List<int> { 1 }
                });

            return pizza.Object;
        }

        private ShoppingCartPizza CreateInvalidShoppingCartPizza()
        {
            var pizza = new Mock<ShoppingCartPizza>();
            pizza.SetupGet(p => p.BasePizzaId).Returns(1);
            pizza.SetupGet(p => p.Quantity).Returns(1);

            // Mock invalid components (non-existent IDs)
            pizza.Setup(p => p.GetComponentsFromJson())
                .Returns(new PizzaComponentsDto
                {
                    DoughId = 999, // Invalid ID
                    SauceId = 999, // Invalid ID
                    SelectedToppings = new List<int> { 999 } // Invalid ID
                });

            return pizza.Object;
        }

        #endregion
    }
}