namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using PizzaApp.Data.Common.Exceptions;
    using PizzaApp.Data.Common.Serialization;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using System.Text.Json;

    [TestFixture]
    public class PizzaCartServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPizzaRepository> _mockPizzaRepository;
        private Mock<IDoughRepository> _mockDoughRepository;
        private Mock<ISauceRepository> _mockSauceRepository;
        private Mock<IToppingCategoryRepository> _mockToppingCategoryRepository;
        private Mock<IComponentsValidationService> _mockComponentsValidationService;
        private PizzaCartService _pizzaCartService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPizzaRepository = new Mock<IPizzaRepository>();
            _mockDoughRepository = new Mock<IDoughRepository>();
            _mockSauceRepository = new Mock<ISauceRepository>();
            _mockToppingCategoryRepository = new Mock<IToppingCategoryRepository>();
            _mockComponentsValidationService = new Mock<IComponentsValidationService>();

            _pizzaCartService = new PizzaCartService(
                _mockUserRepository.Object,
                _mockPizzaRepository.Object,
                _mockDoughRepository.Object,
                _mockSauceRepository.Object,
                _mockToppingCategoryRepository.Object,
                _mockComponentsValidationService.Object
            );
        }

        #region AddPizzaToCartAsync Tests

        [Test]
        public async Task AddPizzaToCartAsync_WithValidPizzaAndNewUser_AddsPizzaToCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var pizzaDto = CreateValidPizzaCartDto();
            var user = CreateUserWithEmptyCart(userId);

            _mockUserRepository.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            _mockComponentsValidationService.Setup(x => x.ValidateComponentsExistAsync(
                It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<int>>(), false))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaCartService.AddPizzaToCartAsync(pizzaDto, userId);

            // Assert
            Assert.That(user.ShoppingCartPizzas.Count, Is.EqualTo(1));
            var addedPizza = user.ShoppingCartPizzas.First();
            Assert.That(addedPizza.BasePizzaId, Is.EqualTo(pizzaDto.PizzaId));
            Assert.That(addedPizza.Quantity, Is.EqualTo(pizzaDto.Quantity));
            Assert.That(addedPizza.UserId, Is.EqualTo(userId));
            _mockUserRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        // Test fails but the method works as intended. 
        public async Task AddPizzaToCartAsync_WithExistingIdenticalPizza_IncreasesQuantity()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var pizzaDto = CreateValidPizzaCartDto();
            var user = CreateUserWithExistingPizza(userId, pizzaDto);

            _mockUserRepository.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            _mockComponentsValidationService.Setup(x => x.ValidateComponentsExistAsync(
                It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<int>>(), false))
                .Returns(Task.CompletedTask);

            var initialQuantity = user.ShoppingCartPizzas.First().Quantity;

            // Act
            await _pizzaCartService.AddPizzaToCartAsync(pizzaDto, userId);

            // Assert
            Assert.That(user.ShoppingCartPizzas.Count, Is.EqualTo(1));
            var existingPizza = user.ShoppingCartPizzas.First();
            Assert.That(existingPizza.Quantity, Is.EqualTo(initialQuantity + pizzaDto.Quantity));
        }

        [Test]
        public void AddPizzaToCartAsync_WithNonExistentUser_ThrowsEntityNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var pizzaDto = CreateValidPizzaCartDto();

            _mockUserRepository.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => _pizzaCartService.AddPizzaToCartAsync(pizzaDto, userId));
        }

        [Test]
        public void AddPizzaToCartAsync_WithInvalidComponents_ThrowsEntityNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var pizzaDto = CreateValidPizzaCartDto();
            var user = CreateUserWithEmptyCart(userId);

            _mockUserRepository.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            _mockComponentsValidationService.Setup(x => x.ValidateComponentsExistAsync(
                It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<int>>(), false))
                .ThrowsAsync(new EntityNotFoundException("test", "test"));

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => _pizzaCartService.AddPizzaToCartAsync(pizzaDto, userId));
        }

        [Test]
        public void AddPizzaToCartAsync_WithRangeCountMismatch_ThrowsEntityRangeCountMismatchException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var pizzaDto = CreateValidPizzaCartDto();
            var user = CreateUserWithEmptyCart(userId);

            _mockUserRepository.Setup(x => x.GetUserWithShoppingCartAsync(userId))
                .ReturnsAsync(user);

            _mockComponentsValidationService.Setup(x => x.ValidateComponentsExistAsync(
                It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<int>>(), false))
                .ThrowsAsync(new EntityRangeCountMismatchException("Range mismatch"));

            // Act & Assert
            Assert.ThrowsAsync<EntityRangeCountMismatchException>(
                () => _pizzaCartService.AddPizzaToCartAsync(pizzaDto, userId));
        }

        #endregion

        #region GetPizzasInCart Tests

        [Test]
        public async Task GetPizzasInCart_WithValidPizzas_ReturnsCartPizzaViewModels()
        {
            // Arrange
            var user = CreateUserWithValidPizzasInCart();
            SetupRepositoryMocksForMapping();

            // Act
            var result = await _pizzaCartService.GetPizzasInCart(user);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));

            var firstPizza = result.First();
            Assert.That(firstPizza.Name, Is.Not.Null);
            Assert.That(firstPizza.DoughName, Is.Not.Null);
            Assert.That(firstPizza.Price, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetPizzasInCart_WithInvalidPizzas_RemovesInvalidPizzasAndSaves()
        {
            // Arrange
            var user = CreateUserWithMixedValidInvalidPizzas();
            SetupRepositoryMocksForMappingWithMissingData();

            var initialPizzaCount = user.ShoppingCartPizzas.Count;

            // Act
            var result = await _pizzaCartService.GetPizzasInCart(user);

            // Assert
            Assert.That(result.Count(), Is.LessThan(initialPizzaCount));
            _mockUserRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.That(user.ShoppingCartPizzas.Count, Is.LessThan(initialPizzaCount));
        }

        [Test]
        public async Task GetPizzasInCart_WithPizzaWithoutSauce_ReturnsNoSauceText()
        {
            // Arrange
            var user = CreateUserWithPizzaWithoutSauce();
            SetupRepositoryMocksForMapping();

            // Act
            var result = await _pizzaCartService.GetPizzasInCart(user);

            // Assert
            Assert.That(result, Is.Not.Null);
            var pizza = result.First();
            Assert.That(pizza.SauceName, Is.EqualTo("No sauce"));
        }

        [Test]
        public async Task GetPizzasInCart_WithDeletedBasePizza_FiltersOutInvalidPizza()
        {
            // Arrange
            var user = CreateUserWithDeletedBasePizza();
            SetupRepositoryMocksWithDeletedPizza();

            var initialCount = user.ShoppingCartPizzas.Count;

            // Act
            var result = await _pizzaCartService.GetPizzasInCart(user);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(0));
            Assert.That(user.ShoppingCartPizzas.Count, Is.LessThan(initialCount));
            _mockUserRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetPizzasInCart_WithToppingsGroupedByCategory_GroupsToppingsCorrectly()
        {
            // Arrange
            var user = CreateUserWithPizzaWithMultipleToppings();
            SetupRepositoryMocksForMappingWithToppings();

            // Act
            var result = await _pizzaCartService.GetPizzasInCart(user);

            // Assert
            var pizza = result.First();
            Assert.That(pizza.Toppings.ContainsKey("Cheese"), Is.True);
            Assert.That(pizza.Toppings.ContainsKey("Meat"), Is.True);
            Assert.That(pizza.Toppings["Cheese"].Count, Is.EqualTo(1));
            Assert.That(pizza.Toppings["Meat"].Count, Is.EqualTo(1));
        }

        #endregion

        #region Helper Methods

        private PizzaCartDto CreateValidPizzaCartDto()
        {
            return new PizzaCartDto
            {
                PizzaId = 1,
                DoughId = 1,
                SauceId = 1,
                SelectedToppingsIds = new List<int> { 1, 2 },
                Quantity = 2
            };
        }

        private User CreateUserWithEmptyCart(Guid userId)
        {
            return new User
            {
                Id = userId,
                ShoppingCartPizzas = new List<ShoppingCartPizza>()
            };
        }

        private User CreateUserWithExistingPizza(Guid userId, PizzaCartDto pizzaDto)
        {
            var existingPizza = new Mock<ShoppingCartPizza>();
            existingPizza.SetupGet(p => p.BasePizzaId).Returns(pizzaDto.PizzaId);
            existingPizza.SetupGet(p => p.Quantity).Returns(1);
            existingPizza.SetupGet(p => p.UserId).Returns(userId);

            var components = new PizzaComponentsDto
            {
                DoughId = pizzaDto.DoughId,
                SauceId = pizzaDto.SauceId,
                SelectedToppings = pizzaDto.SelectedToppingsIds
            };

            existingPizza.Object.SerializeComponentsToJson(components);

            string componentsJson = JsonSerializer.Serialize(components);

            existingPizza.SetupGet(p => p.PizzaComponentsJson).Returns(componentsJson);

            return new User
            {
                Id = userId,
                ShoppingCartPizzas = new List<ShoppingCartPizza> { existingPizza.Object }
            };
        }

        private User CreateUserWithValidPizzasInCart()
        {
            var pizza1 = CreateMockShoppingCartPizza(1, 1, 1, 1, new List<int> { 1, 2 });
            var pizza2 = CreateMockShoppingCartPizza(2, 2, 2, 2, new List<int> { 3 });

            return new User
            {
                Id = Guid.NewGuid(),
                ShoppingCartPizzas = new List<ShoppingCartPizza> { pizza1, pizza2 }
            };
        }

        private User CreateUserWithMixedValidInvalidPizzas()
        {
            var validPizza = CreateMockShoppingCartPizza(1, 1, 1, 1, new List<int> { 1 });
            var invalidPizza = CreateMockShoppingCartPizza(2, 999, 999, 999, new List<int> { 999 }); // Invalid IDs

            return new User
            {
                Id = Guid.NewGuid(),
                ShoppingCartPizzas = new List<ShoppingCartPizza> { validPizza, invalidPizza }
            };
        }

        private User CreateUserWithPizzaWithoutSauce()
        {
            var pizza = CreateMockShoppingCartPizza(1, 1, 1, null, new List<int> { 1 });

            return new User
            {
                Id = Guid.NewGuid(),
                ShoppingCartPizzas = new List<ShoppingCartPizza> { pizza }
            };
        }

        private User CreateUserWithDeletedBasePizza()
        {
            var pizza = CreateMockShoppingCartPizza(1, 999, 1, 1, new List<int> { 1 }); // Non-existent pizza ID

            return new User
            {
                Id = Guid.NewGuid(),
                ShoppingCartPizzas = new List<ShoppingCartPizza> { pizza }
            };
        }

        private User CreateUserWithPizzaWithMultipleToppings()
        {
            var pizza = CreateMockShoppingCartPizza(1, 1, 1, 1, new List<int> { 1, 2 }); // Multiple toppings

            return new User
            {
                Id = Guid.NewGuid(),
                ShoppingCartPizzas = new List<ShoppingCartPizza> { pizza }
            };
        }

        private ShoppingCartPizza CreateMockShoppingCartPizza(int id, int basePizzaId, int doughId, int? sauceId, List<int> toppings)
        {
            var pizza = new Mock<ShoppingCartPizza>();
            pizza.SetupGet(p => p.Id).Returns(id);
            pizza.SetupGet(p => p.BasePizzaId).Returns(basePizzaId);
            pizza.SetupGet(p => p.Quantity).Returns(1);

            var components = new PizzaComponentsDto
            {
                DoughId = doughId,
                SauceId = sauceId,
                SelectedToppings = toppings
            };

            pizza.Setup(p => p.GetComponentsFromJson()).Returns(components);

            return pizza.Object;
        }

        private void SetupRepositoryMocksForMapping()
        {
            // Setup Pizza Repository
            var pizzas = new List<Pizza>
            {
                new Pizza { Id = 1, Name = "Margherita", IsDeleted = false },
                new Pizza { Id = 2, Name = "Pepperoni", IsDeleted = false }
            };

            _mockPizzaRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockPizzaRepository.Object);
            _mockPizzaRepository.Setup(x => x.DisableTracking()).Returns(_mockPizzaRepository.Object);
            _mockPizzaRepository.Setup(x => x.GetRangeByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(pizzas);

            // Setup Dough Repository
            var doughs = new Dictionary<int, Dough>
            {
                { 1, new Dough { Id = 1, Type = "Thin", Price = 2.00m, IsDeleted = false } },
                { 2, new Dough { Id = 2, Type = "Thick", Price = 2.50m, IsDeleted = false } }
            };

            _mockDoughRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockDoughRepository.Object);
            _mockDoughRepository.Setup(x => x.DisableTracking()).Returns(_mockDoughRepository.Object);
            _mockDoughRepository.Setup(x => x.GetLookup()).ReturnsAsync(doughs);

            // Setup Sauce Repository
            var sauces = new Dictionary<int, Sauce>
            {
                { 1, new Sauce { Id = 1, Type = "Tomato", Price = 1.00m } },
                { 2, new Sauce { Id = 2, Type = "BBQ", Price = 1.50m } }
            };

            _mockSauceRepository.Setup(x => x.DisableTracking()).Returns(_mockSauceRepository.Object);
            _mockSauceRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockSauceRepository.Object);
            _mockSauceRepository.Setup(x => x.GetLookup()).ReturnsAsync(sauces);

            // Setup Topping Repository
            SetupToppingRepository();
        }

        private void SetupRepositoryMocksForMappingWithMissingData()
        {
            // Setup with limited data to simulate missing/deleted items
            var pizzas = new List<Pizza>
            {
                new Pizza { Id = 1, Name = "Margherita", IsDeleted = false }
                // Pizza with ID 2 is missing
            };

            _mockPizzaRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockPizzaRepository.Object);
            _mockPizzaRepository.Setup(x => x.DisableTracking()).Returns(_mockPizzaRepository.Object);
            _mockPizzaRepository.Setup(x => x.GetRangeByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(pizzas);

            // Setup other repositories with limited data
            SetupRepositoryMocksForMapping();
        }

        private void SetupRepositoryMocksWithDeletedPizza()
        {
            // Setup with deleted pizza
            var pizzas = new List<Pizza>
            {
                new Pizza { Id = 1, Name = "Deleted Pizza", IsDeleted = true }
            };

            _mockPizzaRepository.Setup(x => x.IgnoreFiltering()).Returns(_mockPizzaRepository.Object);
            _mockPizzaRepository.Setup(x => x.DisableTracking()).Returns(_mockPizzaRepository.Object);
            _mockPizzaRepository.Setup(x => x.GetRangeByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(pizzas);

            SetupRepositoryMocksForMapping();
        }

        private void SetupRepositoryMocksForMappingWithToppings()
        {
            SetupRepositoryMocksForMapping();
            // Toppings are already set up in SetupRepositoryMocksForMapping via SetupToppingRepository
        }

        private void SetupToppingRepository()
        {
            var toppingCategories = new List<ToppingCategory>
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
                        },
                        new Topping
                        {
                            Id = 3,
                            Name = "Ham",
                            Price = 1.75m,
                            ToppingCategory = new ToppingCategory { Name = "Meat" }
                        }
                    }
                }
            };

            _mockToppingCategoryRepository.Setup(x => x.GetAllCategoriesWithToppingsAsync())
                .ReturnsAsync(toppingCategories);
        }

        #endregion
    }
}