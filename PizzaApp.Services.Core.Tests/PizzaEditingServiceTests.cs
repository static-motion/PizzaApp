namespace PizzaApp.Services.Core.Tests
{
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;
    using PizzaApp.Web.ViewModels.MyPizzas;

    [TestFixture]
    public class PizzaEditingServiceTests
    {
        private Mock<IPizzaRepository> _pizzaRepositoryMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IComponentsValidationService> _componentsValidationServiceMock;
        private PizzaEditingService _pizzaEditingService;

        [SetUp]
        public void Setup()
        {
            _pizzaRepositoryMock = new Mock<IPizzaRepository>();

            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _componentsValidationServiceMock = new Mock<IComponentsValidationService>();

            _pizzaEditingService = new PizzaEditingService(
                _pizzaRepositoryMock.Object,
                _userManagerMock.Object,
                _componentsValidationServiceMock.Object);
        }

        [Test]
        public async Task EditPizzaAsync_WithAdminEditingBasePizza_SuccessfullyUpdates()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new EditBasePizzaInputModel
            {
                Id = 1,
                Name = "Updated Pizza",
                Description = "Updated Description",
                DoughId = 1,
                SauceId = 1,
                SelectedToppingIds = new HashSet<int> { 1, 2 },
                ImageUrl = "updated.jpg",
                IsDeleted = false
            };

            var user = new User { Id = userId };
            var existingPizza = new Pizza
            {
                Id = 1,
                PizzaType = PizzaType.AdminPizza,
                Name = "Original Pizza",
                Description = "Original Description",
                DoughId = 2,
                SauceId = 2,
                Toppings = new List<PizzaTopping>(),
                ImageUrl = "original.jpg",
                IsDeleted = true
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(true);

            _pizzaRepositoryMock.Setup(x => x.IgnoreFiltering())
                .Returns(_pizzaRepositoryMock.Object);

            _pizzaRepositoryMock.Setup(x => x.GetByIdWithIngredientsAsync(inputModel.Id))
                .ReturnsAsync(existingPizza);

            _componentsValidationServiceMock.Setup(x => x.ValidateComponentsExistAsync(
                inputModel.DoughId, inputModel.SauceId, inputModel.SelectedToppingIds, true))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaEditingService.EditPizzaAsync(inputModel, userId);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(existingPizza.Name, Is.EqualTo(inputModel.Name));
                Assert.That(existingPizza.Description, Is.EqualTo(inputModel.Description));
                Assert.That(existingPizza.DoughId, Is.EqualTo(inputModel.DoughId));
                Assert.That(existingPizza.SauceId, Is.EqualTo(inputModel.SauceId));
                Assert.That(existingPizza.ImageUrl, Is.EqualTo(inputModel.ImageUrl));
                Assert.That(existingPizza.IsDeleted, Is.EqualTo(inputModel.IsDeleted));
                Assert.That(existingPizza.Toppings.Count, Is.EqualTo(inputModel.SelectedToppingIds.Count));
            });

            _pizzaRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void EditPizzaAsync_WithNonAdminEditingBasePizza_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new EditBasePizzaInputModel
            {
                Id = 1,
                Name = "test"
            };

            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _pizzaEditingService.EditPizzaAsync(inputModel, userId));
        }

        [Test]
        public async Task EditPizzaAsync_WithCustomerEditingOwnPizza_SuccessfullyUpdates()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var inputModel = new EditCustomerPizzaInputModel
            {
                Id = 1,
                Name = "My Updated Pizza",
                Description = "My Updated Description",
                DoughId = 1,
                SauceId = 1,
                SelectedToppingIds = new HashSet<int> { 1 }
            };

            var user = new User { Id = userId };
            var existingPizza = new Pizza
            {
                Id = 1,
                PizzaType = PizzaType.CustomerPizza,
                CreatorUserId = userId,
                Name = "My Original Pizza",
                Description = "My Original Description",
                DoughId = 2,
                SauceId = 2,
                Toppings = new List<PizzaTopping>()
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            _pizzaRepositoryMock.Setup(x => x.GetByIdWithIngredientsAsync(inputModel.Id))
                .ReturnsAsync(existingPizza);

            _componentsValidationServiceMock.Setup(x => x.ValidateComponentsExistAsync(
                inputModel.DoughId, inputModel.SauceId, inputModel.SelectedToppingIds, false))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaEditingService.EditPizzaAsync(inputModel, userId);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(existingPizza.Name, Is.EqualTo(inputModel.Name));
                Assert.That(existingPizza.Description, Is.EqualTo(inputModel.Description));
                Assert.That(existingPizza.DoughId, Is.EqualTo(inputModel.DoughId));
                Assert.That(existingPizza.SauceId, Is.EqualTo(inputModel.SauceId));
                Assert.That(existingPizza.Toppings.Count, Is.EqualTo(inputModel.SelectedToppingIds.Count));
            });

            _pizzaRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void EditPizzaAsync_WithCustomerEditingOthersPizza_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();
            EditCustomerPizzaInputModel inputModel = new EditCustomerPizzaInputModel
            {
                Id = 1,
                Name = "test"
            };

            var user = new User { Id = userId };
            var existingPizza = new Pizza
            {
                Id = 1,
                PizzaType = PizzaType.CustomerPizza,
                CreatorUserId = otherUserId
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            _pizzaRepositoryMock.Setup(x => x.GetByIdWithIngredientsAsync(inputModel.Id))
                .ReturnsAsync(existingPizza);

            // Act & Assert
            Assert.ThrowsAsync<UserNotOwnerException>(() =>
                _pizzaEditingService.EditPizzaAsync(inputModel, userId));
        }

        [Test]
        public async Task GetCustomerPizzaToEdit_WithValidPizza_ReturnsCorrectModel()
        {
            // Arrange
            var pizzaId = 1;
            var userId = Guid.NewGuid();
            var pizza = new Pizza
            {
                Id = pizzaId,
                PizzaType = PizzaType.CustomerPizza,
                CreatorUserId = userId,
                Name = "My Pizza",
                Description = "My Description",
                DoughId = 1,
                SauceId = 1,
                Dough = new Dough { Id = 1, Price = 2m },
                Sauce = new Sauce { Id = 1, Price = 1m },
                Toppings = new List<PizzaTopping>
                {
                    new PizzaTopping { ToppingId = 1, Topping = new Topping { Id = 1, Price = 0.5m } },
                    new PizzaTopping { ToppingId = 2, Topping = new Topping { Id = 2, Price = 0.75m } }
                }
            };

            _pizzaRepositoryMock.Setup(x => x.DisableTracking().GetByIdWithIngredientsAsync(pizzaId))
                .ReturnsAsync(pizza);

            // Act
            var result = await _pizzaEditingService.GetCustomerPizzaToEdit(pizzaId, userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(pizza.Id));
                Assert.That(result.Name, Is.EqualTo(pizza.Name));
                Assert.That(result.Description, Is.EqualTo(pizza.Description));
                Assert.That(result.DoughId, Is.EqualTo(pizza.Dough.Id));
                Assert.That(result.SauceId, Is.EqualTo(pizza.Sauce.Id));
                Assert.That(result.Price, Is.EqualTo(4.25m)); // 2 (dough) + 1 (sauce) + 0.5 + 0.75 (toppings)
                Assert.That(result.SelectedToppingIds.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public void GetCustomerPizzaToEdit_WithNonCustomerPizza_ThrowsException()
        {
            // Arrange
            var pizzaId = 1;
            var userId = Guid.NewGuid();
            var pizza = new Pizza
            {
                Id = pizzaId,
                PizzaType = PizzaType.AdminPizza
            };

            _pizzaRepositoryMock.Setup(x => x.DisableTracking().GetByIdWithIngredientsAsync(pizzaId))
                .ReturnsAsync(pizza);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _pizzaEditingService.GetCustomerPizzaToEdit(pizzaId, userId));
        }

        [Test]
        public async Task GetBasePizzaToEdit_WithAdminUser_ReturnsCorrectModel()
        {
            // Arrange
            var pizzaId = 1;
            var userId = Guid.NewGuid();
            var pizza = new Pizza
            {
                Id = pizzaId,
                CreatorUserId = userId,
                PizzaType = PizzaType.AdminPizza,
                Name = "Base Pizza",
                Description = "Base Description",
                DoughId = 1,
                SauceId = 1,
                Dough = new Dough { Id = 1, Price = 2m },
                Sauce = new Sauce { Id = 1, Price = 1m },
                Toppings = new List<PizzaTopping>
                {
                    new PizzaTopping { Topping = new Topping { Id = 1, Price = 0.5m } }
                },
                ImageUrl = "base.jpg",
                IsDeleted = false
            };

            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(true);

            _pizzaRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().GetByIdWithIngredientsAsync(pizzaId))
                .ReturnsAsync(pizza);

            // Act
            var result = await _pizzaEditingService.GetBasePizzaToEdit(pizzaId, userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(pizza.Id));
                Assert.That(result.Name, Is.EqualTo(pizza.Name));
                Assert.That(result.Description, Is.EqualTo(pizza.Description));
                Assert.That(result.DoughId, Is.EqualTo(pizza.Dough.Id));
                Assert.That(result.SauceId, Is.EqualTo(pizza.Sauce.Id));
                Assert.That(result.Price, Is.EqualTo(3.5m)); // 2 (dough) + 1 (sauce) + 0.5 (topping)
                Assert.That(result.ImageUrl, Is.EqualTo(pizza.ImageUrl));
                Assert.That(result.IsDeleted, Is.EqualTo(pizza.IsDeleted));
                Assert.That(result.SelectedToppingIds.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void GetBasePizzaToEdit_WithNonAdminUser_ThrowsException()
        {
            // Arrange
            var pizzaId = 1;
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _pizzaEditingService.GetBasePizzaToEdit(pizzaId, userId));
        }
    }
}