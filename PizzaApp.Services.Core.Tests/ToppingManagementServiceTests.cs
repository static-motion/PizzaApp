namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    [TestFixture]
    public class ToppingManagementServiceTests
    {
        private Mock<IToppingRepository> _toppingRepositoryMock;
        private Mock<IToppingCategoryRepository> _toppingCategoryRepositoryMock;
        private ToppingManagementService _toppingManagementService;

        [SetUp]
        public void Setup()
        {
            _toppingRepositoryMock = new Mock<IToppingRepository>();
            _toppingCategoryRepositoryMock = new Mock<IToppingCategoryRepository>();
            _toppingManagementService = new ToppingManagementService(
                _toppingRepositoryMock.Object,
                _toppingCategoryRepositoryMock.Object);
        }

        [Test]
        public async Task GetToppingsOverviewPaginatedAsync_ReturnsCorrectPaginationData()
        {
            // Arrange
            var page = 2;
            var pageSize = 5;
            var totalCount = 15;
            var toppings = new List<Topping>
            {
                new Topping {
                    Id = 1,
                    Name = "Pepperoni",
                    IsDeleted = false,
                    ToppingCategory = new ToppingCategory { IsDeleted = false }
                },
                new Topping {
                    Id = 2,
                    Name = "Mushrooms",
                    IsDeleted = true,
                    ToppingCategory = new ToppingCategory { IsDeleted = false }
                },
                new Topping {
                    Id = 3,
                    Name = "Olives",
                    IsDeleted = false,
                    ToppingCategory = new ToppingCategory { IsDeleted = true }
                }
            };

            _toppingRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(totalCount);

            _toppingRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering()
                .TakeWithCategoriesAsync((page - 1) * pageSize, pageSize))
                .ReturnsAsync(toppings);

            // Act
            var result = await _toppingManagementService.GetToppingsOverviewPaginatedAsync(page, pageSize);

            // Assert
            Assert.AreEqual(totalCount, result.Item1);
            Assert.AreEqual(3, result.Item2.Count());

            var firstItem = result.Item2.First();
            Assert.AreEqual(toppings[0].Id, firstItem.Id);
            Assert.AreEqual(toppings[0].Name, firstItem.Name);
            Assert.AreEqual(true, firstItem.IsActive); // Both topping and category not deleted

            var secondItem = result.Item2.ElementAt(1);
            Assert.AreEqual(false, secondItem.IsActive); // Topping deleted

            var thirdItem = result.Item2.ElementAt(2);
            Assert.AreEqual(false, thirdItem.IsActive); // Category deleted

            _toppingRepositoryMock.Verify(x => x.TotalEntityCountAsync(), Times.Once);
            _toppingRepositoryMock.Verify(x => x.DisableTracking().IgnoreFiltering()
                .TakeWithCategoriesAsync((page - 1) * pageSize, pageSize), Times.Once);
        }

        [Test]
        public async Task GetToppingsOverviewPaginatedAsync_WithEmptyRepository_ReturnsZeroCountAndEmptyList()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;

            _toppingRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(0);

            _toppingRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering()
                .TakeWithCategoriesAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Topping>());

            // Act
            var result = await _toppingManagementService.GetToppingsOverviewPaginatedAsync(page, pageSize);

            // Assert
            Assert.AreEqual(0, result.Item1);
            Assert.IsEmpty(result.Item2);
        }

        [Test]
        public async Task GetToppingDetailsByIdAsync_WithValidId_ReturnsCorrectDetails()
        {
            // Arrange
            var toppingId = 1;
            var topping = new Topping
            {
                Id = toppingId,
                Name = "Pepperoni",
                Description = "Spicy sausage",
                Price = 1.50m,
                IsDeleted = false,
                ToppingCategoryId = 1
            };

            var categories = new List<ToppingCategory>
            {
                new ToppingCategory { Id = 1, Name = "Meats", IsDeleted = false },
                new ToppingCategory { Id = 2, Name = "Vegetables", IsDeleted = false }
            };

            _toppingRepositoryMock.Setup(x => x.IgnoreFiltering().DisableTracking().GetByIdAsync(toppingId))
                .ReturnsAsync(topping);

            _toppingCategoryRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().GetAllAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _toppingManagementService.GetToppingDetailsByIdAsync(toppingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(topping.Id, result.ToppingInputModel.Id);
            Assert.AreEqual(topping.Name, result.ToppingInputModel.Name);
            Assert.AreEqual(topping.Description, result.ToppingInputModel.Description);
            Assert.AreEqual(topping.Price, result.ToppingInputModel.Price);
            Assert.AreEqual(topping.IsDeleted, result.ToppingInputModel.IsDeleted);
            Assert.AreEqual(topping.ToppingCategoryId, result.ToppingInputModel.ToppingCategoryId);
            Assert.AreEqual(2, result.AllCategories.Count());

            _toppingRepositoryMock.Verify(x => x.IgnoreFiltering().DisableTracking().GetByIdAsync(toppingId), Times.Once);
            _toppingCategoryRepositoryMock.Verify(x => x.DisableTracking().IgnoreFiltering().GetAllAsync(), Times.Once);
        }

        [Test]
        public void GetToppingDetailsByIdAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var invalidId = 999;
            _toppingRepositoryMock.Setup(x => x.IgnoreFiltering().DisableTracking().GetByIdAsync(invalidId))
                .ReturnsAsync((Topping)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _toppingManagementService.GetToppingDetailsByIdAsync(invalidId));
        }

        [Test]
        public async Task EditToppingAsync_WithValidModel_UpdatesToppingCorrectly()
        {
            // Arrange
            var inputModel = new EditToppingInputModel
            {
                Id = 1,
                Name = "Updated Pepperoni",
                Description = "Updated description",
                Price = 2.00m,
                IsDeleted = true,
                ToppingCategoryId = 2
            };

            var existingTopping = new Topping
            {
                Id = 1,
                Name = "Pepperoni",
                Description = "Original description",
                Price = 1.50m,
                IsDeleted = false,
                ToppingCategoryId = 1
            };

            _toppingRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(inputModel.Id))
                .ReturnsAsync(existingTopping);

            _toppingCategoryRepositoryMock.Setup(x => x.IgnoreFiltering()
                .ExistsAsync(tc => tc.Id == inputModel.ToppingCategoryId))
                .ReturnsAsync(true);

            // Act
            await _toppingManagementService.EditToppingAsync(inputModel);

            // Assert
            Assert.AreEqual(inputModel.Name, existingTopping.Name);
            Assert.AreEqual(inputModel.Description, existingTopping.Description);
            Assert.AreEqual(inputModel.Price, existingTopping.Price);
            Assert.AreEqual(inputModel.IsDeleted, existingTopping.IsDeleted);
            Assert.AreEqual(inputModel.ToppingCategoryId, existingTopping.ToppingCategoryId);

            _toppingRepositoryMock.Verify(x => x.Update(existingTopping), Times.Once);
            _toppingRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void EditToppingAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var inputModel = new EditToppingInputModel 
            {
                Name = "Test",
                Description = "Test",
                Id = 999
            };
            _toppingRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(inputModel.Id))
                .ReturnsAsync((Topping)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _toppingManagementService.EditToppingAsync(inputModel));
        }

        [Test]
        public void EditToppingAsync_WithInvalidCategoryId_ThrowsException()
        {
            // Arrange
            var inputModel = new EditToppingInputModel
            {
                Name = "Test",
                Description = "Test",
                Id = 1,
                ToppingCategoryId = 999 // Invalid category
            };

            var existingTopping = new Topping { Id = 1 };

            _toppingRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(inputModel.Id))
                .ReturnsAsync(existingTopping);

            _toppingCategoryRepositoryMock.Setup(x => x.IgnoreFiltering()
                .ExistsAsync(tc => tc.Id == inputModel.ToppingCategoryId))
                .ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _toppingManagementService.EditToppingAsync(inputModel));
        }
    }
}