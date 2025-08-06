namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using System.Collections.Immutable;

    [TestFixture]
    public class PizzaMenuServiceTests
    {
        private Mock<IPizzaRepository> _pizzaRepositoryMock;
        private Mock<IIngredientsService> _ingredientsServiceMock;
        private PizzaMenuService _pizzaMenuService;

        [SetUp]
        public void Setup()
        {
            _pizzaRepositoryMock = new Mock<IPizzaRepository>();
            _ingredientsServiceMock = new Mock<IIngredientsService>();

            _pizzaMenuService = new PizzaMenuService(
                _pizzaRepositoryMock.Object,
                _ingredientsServiceMock.Object);
        }

        [Test]
        public async Task GetAllBaseItemsAsync_ReturnsAllBasePizzas()
        {
            // Arrange
            var pizzas = new List<Pizza>
            {
                new Pizza { Id = 1, Name = "Margherita", Description = "Classic", ImageUrl = "margherita.jpg", PizzaType = PizzaType.AdminPizza },
                new Pizza { Id = 2, Name = "Pepperoni", Description = "Spicy", ImageUrl = "pepperoni.jpg", PizzaType = PizzaType.AdminPizza }
            };

            _pizzaRepositoryMock.Setup(x => x.DisableTracking().GetAllBasePizzasAsync())
                .ReturnsAsync(pizzas);

            // Act
            var result = await _pizzaMenuService.GetAllBaseItemsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));

            var firstItem = result.First();
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.Id, Is.EqualTo(pizzas[0].Id));
                Assert.That(firstItem.Name, Is.EqualTo(pizzas[0].Name));
                Assert.That(firstItem.Description, Is.EqualTo(pizzas[0].Description));
                Assert.That(firstItem.ImageUrl, Is.EqualTo(pizzas[0].ImageUrl));
                Assert.That(firstItem.Category, Is.EqualTo(MenuCategory.Pizzas));
            });

            _pizzaRepositoryMock.Verify(x => x.DisableTracking().GetAllBasePizzasAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllBaseItemsAsync_WithNoPizzas_ReturnsEmptyList()
        {
            // Arrange
            _pizzaRepositoryMock.Setup(x => x.DisableTracking().GetAllBasePizzasAsync())
                .ReturnsAsync(new List<Pizza>());

            // Act
            var result = await _pizzaMenuService.GetAllBaseItemsAsync();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllUserPizzasAsync_ReturnsUserPizzas()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var pizzas = new List<Pizza>
            {
                new Pizza { Id = 1, Name = "My Pizza", CreatorUserId = userId, PizzaType = PizzaType.CustomerPizza },
                new Pizza { Id = 2, Name = "Another Pizza", CreatorUserId = userId, PizzaType = PizzaType.CustomerPizza }
            };

            _pizzaRepositoryMock.Setup(x => x.DisableTracking().GetAllUserPizzasAsync(userId))
                .ReturnsAsync(pizzas);

            // Act
            var result = await _pizzaMenuService.GetAllUserPizzasAsync(userId);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(result.First().Name, Is.EqualTo(pizzas[0].Name));
                Assert.That(result.Last().Name, Is.EqualTo(pizzas[1].Name));
            });
        }

        [Test]
        public async Task GetAllUserPizzasAsync_WithNoPizzas_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _pizzaRepositoryMock.Setup(x => x.DisableTracking().GetAllUserPizzasAsync(userId))
                .ReturnsAsync(new List<Pizza>());

            // Act
            var result = await _pizzaMenuService.GetAllUserPizzasAsync(userId);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetPizzaDetailsByIdAsync_WithValidId_ReturnsDetailsWithIngredients()
        {
            // Arrange
            var pizzaId = 1;
            var pizza = new Pizza
            {
                Id = pizzaId,
                Name = "Margherita",
                Description = "Classic",
                Dough = new Dough { Id = 1, Price = 2m },
                Sauce = new Sauce { Id = 1, Price = 1m },
                Toppings = new List<PizzaTopping>
                {
                    new PizzaTopping { Topping = new Topping { Id = 1, Price = 0.5m } }
                },
                ImageUrl = "margherita.jpg"
            };

            var ingredients = new PizzaIngredientsViewWrapper
            {
                Doughs = new List<DoughViewModel>(),
                Sauces = new List<SauceViewModel>(),
                ToppingCategories = new List<ToppingCategoryViewWrapper>()
            };

            _pizzaRepositoryMock.Setup(x => x.DisableTracking().GetByIdWithIngredientsAsync(pizzaId))
                .ReturnsAsync(pizza);

            _ingredientsServiceMock.Setup(x => x.GetAllIngredientsAsync(false, false))
                .ReturnsAsync(ingredients);

            // Act
            var result = await _pizzaMenuService.GetPizzaDetailsByIdAsync(pizzaId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Pizza.Name, Is.EqualTo(pizza.Name));
                Assert.That(result.Pizza.Price, Is.EqualTo(3.5m)); // 2 (dough) + 1 (sauce) + 0.5 (topping)
                Assert.That(result.Ingredients, Is.EqualTo(ingredients));
            });
        }

        [Test]
        public void GetPizzaDetailsByIdAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var invalidId = 999;
            _pizzaRepositoryMock.Setup(x => x.DisableTracking().GetByIdWithIngredientsAsync(invalidId))
                .ReturnsAsync((Pizza)null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _pizzaMenuService.GetPizzaDetailsByIdAsync(invalidId));
        }
    }
}