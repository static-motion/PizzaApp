namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using NUnit.Framework;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class DrinkMenuServiceTests
    {
        private Mock<IDrinkRepository> _drinkRepositoryMock;
        private DrinkMenuService _drinkMenuService;

        [SetUp]
        public void Setup()
        {
            _drinkRepositoryMock = new Mock<IDrinkRepository>();
            _drinkMenuService = new DrinkMenuService(_drinkRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllBaseItemsAsync_ReturnsAllDrinksAsMenuItems()
        {
            // Arrange
            var drinks = new List<Drink>
            {
                new Drink { Id = 1, Name = "Cola", Description = "Refreshing cola", ImageUrl = "cola.jpg" },
                new Drink { Id = 2, Name = "Lemonade", Description = "Sweet lemonade", ImageUrl = "lemonade.jpg" }
            };

            _drinkRepositoryMock.Setup(x => x.DisableTracking().GetAllAsync())
                .ReturnsAsync(drinks);

            // Act
            var result = await _drinkMenuService.GetAllBaseItemsAsync();

            // Assert
            var firstItem = result.First();
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(firstItem.Id, Is.EqualTo(drinks[0].Id));
                Assert.That(firstItem.Name, Is.EqualTo(drinks[0].Name));
                Assert.That(firstItem.Description, Is.EqualTo(drinks[0].Description));
                Assert.That(firstItem.ImageUrl, Is.EqualTo(drinks[0].ImageUrl));
                Assert.That(firstItem.Category, Is.EqualTo(MenuCategory.Drinks));
            });

            _drinkRepositoryMock.Verify(x => x.DisableTracking().GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllBaseItemsAsync_WhenNoDrinksExist_ReturnsEmptyList()
        {
            // Arrange
            _drinkRepositoryMock.Setup(x => x.DisableTracking().GetAllAsync())
                .ReturnsAsync(new List<Drink>());

            // Act
            var result = await _drinkMenuService.GetAllBaseItemsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetDetailsById_WithValidId_ReturnsDrinkDetails()
        {
            // Arrange
            var drinkId = 1;
            var drink = new Drink
            {
                Id = drinkId,
                Name = "Cola",
                Description = "Refreshing cola",
                ImageUrl = "cola.jpg",
                Price = 2.50m
            };

            _drinkRepositoryMock.Setup(x => x.GetByIdAsync(drinkId))
                .ReturnsAsync(drink);

            // Act
            var result = await _drinkMenuService.GetDetailsById(drinkId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(drink.Id));
                Assert.That(result.Name, Is.EqualTo(drink.Name));
                Assert.That(result.Description, Is.EqualTo(drink.Description));
                Assert.That(result.ImageUrl, Is.EqualTo(drink.ImageUrl));
                Assert.That(result.Price, Is.EqualTo(drink.Price));
                Assert.That(result.Category, Is.EqualTo(MenuCategory.Drinks));
            });

            _drinkRepositoryMock.Verify(x => x.GetByIdAsync(drinkId), Times.Once);
        }

        [Test]
        public void GetDetailsById_WithInvalidId_ThrowsEntityNotFoundException()
        {
            // Arrange
            var invalidId = 999;
            _drinkRepositoryMock.Setup(x => x.GetByIdAsync(invalidId))
                .ReturnsAsync((Drink)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _drinkMenuService.GetDetailsById(invalidId));
        }

        [Test]
        public void GetDetailsById_WithZeroId_ThrowsEntityNotFoundException()
        {
            // Arrange
            var zeroId = 0;
            _drinkRepositoryMock.Setup(x => x.GetByIdAsync(zeroId))
                .ReturnsAsync((Drink)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _drinkMenuService.GetDetailsById(zeroId));
        }

        [Test]
        public void GetDetailsById_WithNegativeId_ThrowsEntityNotFoundException()
        {
            // Arrange
            var negativeId = -1;
            _drinkRepositoryMock.Setup(x => x.GetByIdAsync(negativeId))
                .ReturnsAsync((Drink)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _drinkMenuService.GetDetailsById(negativeId));
        }

        [Test]
        public async Task GetAllBaseItemsAsync_VerifyTrackingIsDisabled()
        {
            // Arrange
            _drinkRepositoryMock.Setup(x => x.DisableTracking().GetAllAsync())
                .ReturnsAsync(new List<Drink>());

            // Act
            await _drinkMenuService.GetAllBaseItemsAsync();

            // Assert
            _drinkRepositoryMock.Verify(x => x.DisableTracking(), Times.Once);
        }

        [Test]
        public async Task GetDetailsById_VerifyTrackingBehavior()
        {
            // Arrange
            var drinkId = 1;
            _drinkRepositoryMock.Setup(x => x.GetByIdAsync(drinkId))
                .ReturnsAsync(new Drink { Id = drinkId });

            // Act
            await _drinkMenuService.GetDetailsById(drinkId);

            // Assert
            _drinkRepositoryMock.Verify(x => x.DisableTracking(), Times.Never);
        }
    }
}