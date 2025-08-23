namespace PizzaApp.Services.Core.Tests
{
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using NUnit.Framework;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;
    using PizzaApp.Web.ViewModels.Pizzas;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class PizzaCreationServiceTests
    {
        private Mock<IPizzaRepository> _pizzaRepositoryMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IComponentsValidationService> _componentsValidationServiceMock;
        private PizzaCreationService _pizzaCreationService;

        [SetUp]
        public void Setup()
        {
            _pizzaRepositoryMock = new Mock<IPizzaRepository>();

            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _componentsValidationServiceMock = new Mock<IComponentsValidationService>();

            _pizzaCreationService = new PizzaCreationService(
                _pizzaRepositoryMock.Object,
                _userManagerMock.Object,
                _componentsValidationServiceMock.Object);
        }

        [Test]
        public async Task CreatePizzaAsync_WithAdminCreatingBasePizza_SuccessfullyCreates()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new CreateBasePizzaInputModel
            {
                Name = "New Base Pizza",
                Description = "Base Pizza Description",
                DoughId = 1,
                SauceId = 1,
                SelectedToppingIds = new HashSet<int> { 1, 2 },
                ImageUrl = "base.jpg",
                IsDeleted = false
            };

            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(true);

            _componentsValidationServiceMock.Setup(x => x.ValidateComponentsExistAsync(
                inputModel.DoughId, inputModel.SauceId, inputModel.SelectedToppingIds, true))
                .Returns(Task.CompletedTask);

            _pizzaRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Pizza>()))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaCreationService.CreatePizzaAsync(inputModel, userId);

            // Assert
            _pizzaRepositoryMock.Verify(x => x.AddAsync(It.Is<Pizza>(p =>
                p.Name == inputModel.Name &&
                p.Description == inputModel.Description &&
                p.DoughId == inputModel.DoughId &&
                p.SauceId == inputModel.SauceId &&
                p.PizzaType == inputModel.PizzaType &&
                p.ImageUrl == inputModel.ImageUrl &&
                p.IsDeleted == inputModel.IsDeleted &&
                p.Toppings.Count == inputModel.SelectedToppingIds.Count)),
                Times.Once);

            _pizzaRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CreatePizzaAsync_WithCustomerCreatingCustomPizza_SuccessfullyCreates()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new CreateCustomerPizzaInputModel
            {
                Name = "My Custom Pizza",
                Description = "My Pizza Description",
                DoughId = 1,
                SauceId = 1,
                SelectedToppingIds = new HashSet<int> { 1 }
            };

            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            _componentsValidationServiceMock.Setup(x => x.ValidateComponentsExistAsync(
                inputModel.DoughId, inputModel.SauceId, inputModel.SelectedToppingIds, false))
                .Returns(Task.CompletedTask);

            _pizzaRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Pizza>()))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaCreationService.CreatePizzaAsync(inputModel, userId);

            // Assert
            _pizzaRepositoryMock.Verify(x => x.AddAsync(It.Is<Pizza>(p =>
                p.CreatorUserId == userId &&
                p.PizzaType == PizzaType.CustomerPizza &&
                p.Toppings.Count == 1)),
                Times.Once);

            _pizzaRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void CreatePizzaAsync_WithNonAdminCreatingBasePizza_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new CreateBasePizzaInputModel
            {
                Name = "test"
            };

            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _pizzaCreationService.CreatePizzaAsync(inputModel, userId));
        }

        [Test]
        public void CreatePizzaAsync_WithInvalidUser_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new CreateCustomerPizzaInputModel()
            {
                Name = "test"
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _pizzaCreationService.CreatePizzaAsync(inputModel, userId));
        }

        [Test]
        public void CreatePizzaAsync_WithInvalidComponents_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new CreateCustomerPizzaInputModel
            {
                Name = "Test",
                DoughId = 1,
                SauceId = 1,
                SelectedToppingIds = new HashSet<int> { 999 } // Invalid topping
            };

            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            _componentsValidationServiceMock.Setup(x => x.ValidateComponentsExistAsync(
                inputModel.DoughId, inputModel.SauceId, inputModel.SelectedToppingIds, false))
                .ThrowsAsync(new EntityNotFoundException("test", "test"));

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _pizzaCreationService.CreatePizzaAsync(inputModel, userId));
        }

        [Test]
        public async Task CreatePizzaAsync_WithBasePizza_SetsImageUrlAndIsDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new CreateBasePizzaInputModel
            {
                Name = "test",
                ImageUrl = "test.jpg",
                IsDeleted = true,
                DoughId = 1,
                SelectedToppingIds = new HashSet<int>()
            };

            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(true);

            _componentsValidationServiceMock.Setup(x => x.ValidateComponentsExistAsync(
                It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<HashSet<int>>(), true))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaCreationService.CreatePizzaAsync(inputModel, userId);

            // Assert
            _pizzaRepositoryMock.Verify(x => x.AddAsync(It.Is<Pizza>(p =>
                p.ImageUrl == inputModel.ImageUrl &&
                p.IsDeleted == inputModel.IsDeleted)),
                Times.Once);
        }

        [Test]
        public async Task CreatePizzaAsync_WithCustomerPizza_DoesNotSetImageUrlAndIsDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new CreateCustomerPizzaInputModel
            {
                Name = "Test",
                DoughId = 1,
                SelectedToppingIds = new HashSet<int>()
            };

            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            _componentsValidationServiceMock.Setup(x => x.ValidateComponentsExistAsync(
                It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<HashSet<int>>(), false))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaCreationService.CreatePizzaAsync(inputModel, userId);

            // Assert
            _pizzaRepositoryMock.Verify(x => x.AddAsync(It.Is<Pizza>(p =>
                p.ImageUrl == null &&
                p.IsDeleted == false)),
                Times.Once);
        }
    }
}