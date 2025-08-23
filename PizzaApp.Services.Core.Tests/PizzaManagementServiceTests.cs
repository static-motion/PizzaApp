namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using PizzaApp.Data.Common.Exceptions;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    [TestFixture]
    public class PizzaManagementServiceTests
    {
        private Mock<IPizzaRepository> _pizzaRepositoryMock;
        private Mock<IPizzaCreationService> _pizzaCreationServiceMock;
        private Mock<IPizzaEditingService> _pizzaEditingServiceMock;
        private Mock<IPizzaActiveStatusService> _pizzaActiveStatusServiceMock;
        private PizzaManagementService _pizzaManagementService;

        [SetUp]
        public void Setup()
        {
            _pizzaRepositoryMock = new Mock<IPizzaRepository>();
            _pizzaCreationServiceMock = new Mock<IPizzaCreationService>();
            _pizzaEditingServiceMock = new Mock<IPizzaEditingService>();
            _pizzaActiveStatusServiceMock = new Mock<IPizzaActiveStatusService>();

            _pizzaManagementService = new PizzaManagementService(
                _pizzaRepositoryMock.Object,
                _pizzaCreationServiceMock.Object,
                _pizzaEditingServiceMock.Object,
                _pizzaActiveStatusServiceMock.Object);
        }

        [Test]
        public async Task GetPizzasOverviewPaginatedAsync_ReturnsCorrectPaginationData()
        {
            // Arrange
            var page = 2;
            var pageSize = 5;
            var totalCount = 15;
            var pizzas = new List<Pizza>
            {
                new Pizza { Id = 1, Name = "Margherita", IsDeleted = false },
                new Pizza { Id = 2, Name = "Pepperoni", IsDeleted = true },
                new Pizza { Id = 3, Name = "Hawaiian", IsDeleted = false }
            };

            
            
            _pizzaRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(totalCount);

            _pizzaRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().TakeBasePizzasWithIngredientsAsync(
                (page - 1) * pageSize, pageSize))
                .ReturnsAsync(pizzas);

            // Act
            var result = await _pizzaManagementService.GetPizzasOverviewPaginatedAsync(page, pageSize);
            var firstItem = result.Item2.First();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Item1, Is.EqualTo(totalCount));
                Assert.That(result.Item2.Count(), Is.EqualTo(3));
                Assert.That(firstItem.Id, Is.EqualTo(pizzas[0].Id));
                Assert.That(firstItem.Name, Is.EqualTo(pizzas[0].Name));
            });

            _pizzaRepositoryMock.Verify(x => x.TotalEntityCountAsync(), Times.Once);
            _pizzaRepositoryMock.Verify(x => x.DisableTracking().IgnoreFiltering()
                .TakeBasePizzasWithIngredientsAsync((page - 1) * pageSize, pageSize), Times.Once);
        }

        [Test]
        public async Task GetPizzasOverviewPaginatedAsync_WithEmptyRepository_ReturnsZeroCountAndEmptyList()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;

            _pizzaRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(0);

            _pizzaRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering()
                .TakeBasePizzasWithIngredientsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Pizza>());

            // Act
            var result = await _pizzaManagementService.GetPizzasOverviewPaginatedAsync(page, pageSize);

            // Assert
            Assert.That(result.Item1, Is.EqualTo(0));
            Assert.That(result.Item2, Is.Empty);
        }

        [Test]
        public async Task GetBasePizzaAsync_DelegatesToPizzaEditingService()
        {
            // Arrange
            var pizzaId = 1;
            var userId = Guid.NewGuid();
            var expectedModel = new EditBasePizzaInputModel 
            { 
                Id = pizzaId,
                Name = "Test"
            };

            _pizzaEditingServiceMock.Setup(x => x.GetBasePizzaToEdit(pizzaId, userId))
                .ReturnsAsync(expectedModel);

            // Act
            var result = await _pizzaManagementService.GetBasePizzaAsync(pizzaId, userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedModel));
            _pizzaEditingServiceMock.Verify(x => x.GetBasePizzaToEdit(pizzaId, userId), Times.Once);
        }

        [Test]
        public void GetBasePizzaAsync_WhenEditingServiceThrows_PropagatesException()
        {
            // Arrange
            var pizzaId = 1;
            var userId = Guid.NewGuid();

            _pizzaEditingServiceMock.Setup(x => x.GetBasePizzaToEdit(pizzaId, userId))
                .ThrowsAsync(new InvalidOperationException());

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _pizzaManagementService.GetBasePizzaAsync(pizzaId, userId));
        }

        [Test]
        public async Task EditPizzaAsync_DelegatesToPizzaEditingService()
        {
            // Arrange
            var inputModel = new EditBasePizzaInputModel 
            {
                Id = 1,
                Name = "Test"
            };
            var userId = Guid.NewGuid();

            _pizzaEditingServiceMock.Setup(x => x.EditPizzaAsync(inputModel, userId))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaManagementService.EditPizzaAsync(inputModel, userId);

            // Assert
            _pizzaEditingServiceMock.Verify(x => x.EditPizzaAsync(inputModel, userId), Times.Once);
        }

        [Test]
        public void EditPizzaAsync_WhenEditingServiceThrows_PropagatesException()
        {
            // Arrange
            var inputModel = new EditBasePizzaInputModel 
            { 
                Name = "test", 
                Id = 1 
            };
            var userId = Guid.NewGuid();

            _pizzaEditingServiceMock.Setup(x => x.EditPizzaAsync(inputModel, userId))
                .ThrowsAsync(new InvalidOperationException());

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _pizzaManagementService.EditPizzaAsync(inputModel, userId));
        }

        [Test]
        public async Task CreateBasePizzaAsync_DelegatesToPizzaCreationService()
        {
            // Arrange
            var inputModel = new CreateBasePizzaInputModel() { Name = "Test"};
            var userId = Guid.NewGuid();

            _pizzaCreationServiceMock.Setup(x => x.CreatePizzaAsync(inputModel, userId))
                .Returns(Task.CompletedTask);

            // Act
            await _pizzaManagementService.CreateBasePizzaAsync(inputModel, userId);

            // Assert
            _pizzaCreationServiceMock.Verify(x => x.CreatePizzaAsync(inputModel, userId), Times.Once);
        }

        [Test]
        public void CreateBasePizzaAsync_WhenCreationServiceThrowsEntityRangeCountMismatch_PropagatesException()
        {
            // Arrange
            var inputModel = new CreateBasePizzaInputModel() { Name = "Test" };
            var userId = Guid.NewGuid();

            _pizzaCreationServiceMock.Setup(x => x.CreatePizzaAsync(inputModel, userId))
                .ThrowsAsync(new EntityRangeCountMismatchException(""));

            // Act & Assert
            Assert.ThrowsAsync<EntityRangeCountMismatchException>(() =>
                _pizzaManagementService.CreateBasePizzaAsync(inputModel, userId));
        }

        [Test]
        public void CreateBasePizzaAsync_WhenCreationServiceThrowsEntityNotFound_PropagatesException()
        {
            // Arrange
            var inputModel = new CreateBasePizzaInputModel() { Name = "Test" };
            var userId = Guid.NewGuid();

            _pizzaCreationServiceMock.Setup(x => x.CreatePizzaAsync(inputModel, userId))
                .ThrowsAsync(new EntityNotFoundException("test", "test"));

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _pizzaManagementService.CreateBasePizzaAsync(inputModel, userId));
        }
    }
}