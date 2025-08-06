using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PizzaApp.Data.Models;
using PizzaApp.Data.Repository.Interfaces;
using PizzaApp.Services.Core;
using PizzaApp.Services.Core.Interfaces;
using PizzaApp.Web.ViewModels.Admin;
using PizzaApp.Web.ViewModels.Menu;

namespace PizzaApp.Services.Core.Tests
{
    [TestFixture]
    public class ToppingCategoryManagementServiceTests
    {
        private Mock<IToppingCategoryRepository> _toppingCategoryRepositoryMock;
        private ToppingCategoryManagementService _toppingCategoryManagementService;

        [SetUp]
        public void Setup()
        {
            _toppingCategoryRepositoryMock = new Mock<IToppingCategoryRepository>();
            _toppingCategoryManagementService = new ToppingCategoryManagementService(_toppingCategoryRepositoryMock.Object);
        }

        [Test]
        public async Task GetToppingCategoriesOverviewPaginatedAsync_ReturnsCorrectPaginationData()
        {
            // Arrange
            var page = 2;
            var pageSize = 5;
            var totalCount = 15;
            var categories = new List<ToppingCategory>
            {
                new ToppingCategory { Id = 1, Name = "Vegetables", IsDeleted = false },
                new ToppingCategory { Id = 2, Name = "Meats", IsDeleted = true },
                new ToppingCategory { Id = 3, Name = "Cheeses", IsDeleted = false }
            };

            _toppingCategoryRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(totalCount);

            _toppingCategoryRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().TakeAsync(
                (page - 1) * pageSize, pageSize))
                .ReturnsAsync(categories);

            // Act
            var result = await _toppingCategoryManagementService.GetToppingCategoriesOverviewPaginatedAsync(page, pageSize);
            var firstItem = result.Item2.First();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Item1, Is.EqualTo(totalCount));
                Assert.That(result.Item2.Count(), Is.EqualTo(3));
                Assert.That(firstItem.Id, Is.EqualTo(categories[0].Id));
                Assert.That(firstItem.Name, Is.EqualTo(categories[0].Name));
                Assert.That(firstItem.IsActive, Is.EqualTo(!categories[0].IsDeleted));
            });

            _toppingCategoryRepositoryMock.Verify(x => x.TotalEntityCountAsync(), Times.Once);
            _toppingCategoryRepositoryMock.Verify(x => x.DisableTracking().IgnoreFiltering()
                .TakeAsync((page - 1) * pageSize, pageSize), Times.Once);
        }

        [Test]
        public async Task GetToppingCategoriesOverviewPaginatedAsync_WithEmptyRepository_ReturnsZeroCountAndEmptyList()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;

            _toppingCategoryRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(0);

            _toppingCategoryRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering()
                .TakeAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<ToppingCategory>());

            // Act
            var result = await _toppingCategoryManagementService.GetToppingCategoriesOverviewPaginatedAsync(page, pageSize);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Item1, Is.EqualTo(0));
                Assert.That(result.Item2, Is.Empty);
            });
        }

        [Test]
        public async Task GetToppingCategoryDetailsByIdAsync_WithValidId_ReturnsCorrectDetails()
        {
            // Arrange
            var categoryId = 1;
            var category = new ToppingCategory
            {
                Id = categoryId,
                Name = "Vegetables",
                IsDeleted = false
            };

            _toppingCategoryRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            // Act
            var result = await _toppingCategoryManagementService.GetToppingCategoryDetailsByIdAsync(categoryId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(category.Id));
                Assert.That(result.Name, Is.EqualTo(category.Name));
                Assert.That(result.IsDeleted, Is.EqualTo(category.IsDeleted));
            });

            _toppingCategoryRepositoryMock.Verify(x => x.DisableTracking().IgnoreFiltering().GetByIdAsync(categoryId), Times.Once);
        }

        [Test]
        public void GetToppingCategoryDetailsByIdAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var invalidId = 999;
            _toppingCategoryRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().GetByIdAsync(invalidId))
                .ReturnsAsync((ToppingCategory)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _toppingCategoryManagementService.GetToppingCategoryDetailsByIdAsync(invalidId));
        }

        [Test]
        public async Task EditToppingCategoryAsync_WithValidModel_UpdatesCategoryCorrectly()
        {
            // Arrange
            var inputModel = new EditToppingCategoryInputModel
            {
                Id = 1,
                Name = "Updated Vegetables",
                IsDeleted = true
            };

            var existingCategory = new ToppingCategory
            {
                Id = 1,
                Name = "Vegetables",
                IsDeleted = false
            };

            _toppingCategoryRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(inputModel.Id))
                .ReturnsAsync(existingCategory);

            // Act
            await _toppingCategoryManagementService.EditToppingCategoryAsync(inputModel);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(existingCategory.Name, Is.EqualTo(inputModel.Name));
                Assert.That(existingCategory.IsDeleted, Is.EqualTo(inputModel.IsDeleted));
            });

            _toppingCategoryRepositoryMock.Verify(x => x.Update(existingCategory), Times.Once);
            _toppingCategoryRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void EditToppingCategoryAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var inputModel = new EditToppingCategoryInputModel { Name = "test", Id = 999 };
            _toppingCategoryRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(inputModel.Id))
                .ReturnsAsync((ToppingCategory)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _toppingCategoryManagementService.EditToppingCategoryAsync(inputModel));
        }

        [Test]
        public async Task EditToppingCategoryAsync_VerifyTrackingBehavior()
        {
            // Arrange
            var inputModel = new EditToppingCategoryInputModel { Name = "test", Id = 1 };
            var existingCategory = new ToppingCategory { Id = 1 };

            _toppingCategoryRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(inputModel.Id))
                .ReturnsAsync(existingCategory);

            // Act
            await _toppingCategoryManagementService.EditToppingCategoryAsync(inputModel);

            // Assert
            _toppingCategoryRepositoryMock.Verify(x => x.IgnoreFiltering(), Times.Once);
            _toppingCategoryRepositoryMock.Verify(x => x.DisableTracking(), Times.Never);
        }
    }
}