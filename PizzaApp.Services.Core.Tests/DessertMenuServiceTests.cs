namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core;

    [TestFixture]
    public class DessertMenuServiceTests
    {
        private Mock<IDessertRepository> _dessertRepositoryMock;
        private DessertMenuService _dessertMenuService;

        [SetUp]
        public void Setup()
        {
            _dessertRepositoryMock = new Mock<IDessertRepository>();
            _dessertMenuService = new DessertMenuService(_dessertRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllBaseItemsAsync_ReturnsAllDessertsAsMenuItems()
        {
            // Arrange
            var desserts = new List<Dessert>
            {
                new Dessert { Id = 1, Name = "Chocolate Cake", Description = "Delicious cake", ImageUrl = "cake.jpg" },
                new Dessert { Id = 2, Name = "Ice Cream", Description = "Vanilla flavor", ImageUrl = "icecream.jpg" }
            };

            _dessertRepositoryMock.Setup(x => x.DisableTracking().GetAllAsync())
                .ReturnsAsync(desserts);

            // Act
            var result = await _dessertMenuService.GetAllBaseItemsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));

            var firstItem = result.First();
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.Id, Is.EqualTo(desserts[0].Id));
                Assert.That(firstItem.Name, Is.EqualTo(desserts[0].Name));
                Assert.That(firstItem.Description, Is.EqualTo(desserts[0].Description));
                Assert.That(firstItem.ImageUrl, Is.EqualTo(desserts[0].ImageUrl));
                Assert.That(firstItem.Category, Is.EqualTo(MenuCategory.Desserts));
            });

            _dessertRepositoryMock.Verify(x => x.DisableTracking().GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllBaseItemsAsync_WhenNoDessertsExist_ReturnsEmptyList()
        {
            // Arrange
            _dessertRepositoryMock.Setup(x => x.DisableTracking().GetAllAsync())
                .ReturnsAsync(new List<Dessert>());

            // Act
            var result = await _dessertMenuService.GetAllBaseItemsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetDetailsById_WithValidId_ReturnsDessertDetails()
        {
            // Arrange
            var dessertId = 1;
            var dessert = new Dessert
            {
                Id = dessertId,
                Name = "Chocolate Cake",
                Description = "Delicious cake",
                ImageUrl = "cake.jpg",
                Price = 5.99m
            };

            _dessertRepositoryMock.Setup(x => x.GetByIdAsync(dessertId))
                .ReturnsAsync(dessert);

            // Act
            var result = await _dessertMenuService.GetDetailsById(dessertId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(dessert.Id));
                Assert.That(result.Name, Is.EqualTo(dessert.Name));
                Assert.That(result.Description, Is.EqualTo(dessert.Description));
                Assert.That(result.ImageUrl, Is.EqualTo(dessert.ImageUrl));
                Assert.That(result.Price, Is.EqualTo(dessert.Price));
                Assert.That(result.Category, Is.EqualTo(MenuCategory.Desserts));
            });

            _dessertRepositoryMock.Verify(x => x.GetByIdAsync(dessertId), Times.Once);
        }

        [Test]
        public void GetDetailsById_WithInvalidId_ThrowsEntityNotFoundException()
        {
            // Arrange
            var invalidId = 999;
            _dessertRepositoryMock.Setup(x => x.GetByIdAsync(invalidId))
                .ReturnsAsync((Dessert)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _dessertMenuService.GetDetailsById(invalidId));
        }

        [Test]
        public void GetDetailsById_WithZeroId_ThrowsEntityNotFoundException()
        {
            // Arrange
            var zeroId = 0;
            _dessertRepositoryMock.Setup(x => x.GetByIdAsync(zeroId))
                .ReturnsAsync((Dessert)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _dessertMenuService.GetDetailsById(zeroId));
        }

        [Test]
        public void GetDetailsById_WithNegativeId_ThrowsEntityNotFoundException()
        {
            // Arrange
            var negativeId = -1;
            _dessertRepositoryMock.Setup(x => x.GetByIdAsync(negativeId))
                .ReturnsAsync((Dessert)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _dessertMenuService.GetDetailsById(negativeId));
        }
    }
}