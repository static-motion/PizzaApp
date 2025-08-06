namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using NUnit.Framework;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core;
    using PizzaApp.Web.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class SauceManagementServiceTests
    {
        private Mock<ISauceRepository> _sauceRepositoryMock;
        private SauceManagementService _sauceManagementService;

        [SetUp]
        public void Setup()
        {
            _sauceRepositoryMock = new Mock<ISauceRepository>();
            _sauceManagementService = new SauceManagementService(_sauceRepositoryMock.Object);
        }

        [Test]
        public async Task GetSaucesOverviewPaginatedAsync_ReturnsCorrectPaginationData()
        {
            // Arrange
            var page = 2;
            var pageSize = 5;
            var totalCount = 15;
            var sauces = new List<Sauce>
            {
                new Sauce { Id = 6, Type = "Tomato", IsDeleted = false },
                new Sauce { Id = 7, Type = "Garlic", IsDeleted = true },
                new Sauce { Id = 8, Type = "BBQ", IsDeleted = false }
            };

            _sauceRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(totalCount);

            _sauceRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().TakeAsync((page - 1) * pageSize, pageSize))
                .ReturnsAsync(sauces);

            // Act
            var result = await _sauceManagementService.GetSaucesOverviewPaginatedAsync(page, pageSize);

            var firstItem = result.Item2.First();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Item1, Is.EqualTo(totalCount));
                Assert.That(result.Item2.Count(), Is.EqualTo(3));
                Assert.That(firstItem.Id, Is.EqualTo(sauces[0].Id));
                Assert.That(firstItem.Name, Is.EqualTo(sauces[0].Type));
                Assert.That(firstItem.IsActive, Is.EqualTo(!sauces[0].IsDeleted));
            });

            _sauceRepositoryMock.Verify(x => x.TotalEntityCountAsync(), Times.Once);
            _sauceRepositoryMock.Verify(x => x.DisableTracking().IgnoreFiltering().TakeAsync((page - 1) * pageSize, pageSize), Times.Once);
        }

        [Test]
        public async Task GetSaucesOverviewPaginatedAsync_WithEmptyRepository_ReturnsZeroCountAndEmptyList()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;

            _sauceRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(0);

            _sauceRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().TakeAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Sauce>());

            // Act
            var result = await _sauceManagementService.GetSaucesOverviewPaginatedAsync(page, pageSize);

            // Assert
            Assert.That(result.Item1, Is.EqualTo(0));
            Assert.IsEmpty(result.Item2);
        }

        [Test]
        public async Task GetSauceDetailsByIdAsync_WithValidId_ReturnsCorrectDetails()
        {
            // Arrange
            var sauceId = 1;
            var sauce = new Sauce
            {
                Id = sauceId,
                Type = "Tomato",
                Description = "Classic tomato sauce",
                Price = 1.50m,
                IsDeleted = false
            };

            _sauceRepositoryMock.Setup(x => x.IgnoreFiltering().DisableTracking().GetByIdAsync(sauceId))
                .ReturnsAsync(sauce);

            // Act
            var result = await _sauceManagementService.GetSauceDetailsByIdAsync(sauceId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(sauce.Id));
                Assert.That(result.Type, Is.EqualTo(sauce.Type));
                Assert.That(result.Description, Is.EqualTo(sauce.Description));
                Assert.That(result.Price, Is.EqualTo(sauce.Price));
                Assert.That(result.IsDeleted, Is.EqualTo(sauce.IsDeleted));
            });

            _sauceRepositoryMock.Verify(x => x.IgnoreFiltering().DisableTracking().GetByIdAsync(sauceId), Times.Once);
        }

        [Test]
        public void GetSauceDetailsByIdAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var invalidId = 999;
            _sauceRepositoryMock.Setup(x => x.IgnoreFiltering().DisableTracking().GetByIdAsync(invalidId))
                .ReturnsAsync((Sauce)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _sauceManagementService.GetSauceDetailsByIdAsync(invalidId));
        }

        [Test]
        public async Task EditSauceAsync_WithValidModel_UpdatesSauceCorrectly()
        {
            // Arrange
            var inputModel = new EditSauceInputModel
            {
                Id = 1,
                Type = "New Tomato",
                Description = "Updated description",
                Price = 2.00m,
                IsDeleted = true
            };

            var existingSauce = new Sauce
            {
                Id = 1,
                Type = "Old Tomato",
                Description = "Old description",
                Price = 1.50m,
                IsDeleted = false
            };

            _sauceRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(inputModel.Id))
                .ReturnsAsync(existingSauce);

            // Act
            await _sauceManagementService.EditSauceAsync(inputModel);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(existingSauce.Type, Is.EqualTo(inputModel.Type));
                Assert.That(existingSauce.Description, Is.EqualTo(inputModel.Description));
                Assert.That(existingSauce.Price, Is.EqualTo(inputModel.Price));
                Assert.That(existingSauce.IsDeleted, Is.EqualTo(inputModel.IsDeleted));
            });

            _sauceRepositoryMock.Verify(x => x.Update(existingSauce), Times.Once);
            _sauceRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void EditSauceAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var inputModel = new EditSauceInputModel 
            { 
                Type = "test",
                Description = "test", 
                Id = 999 
            };
            _sauceRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(inputModel.Id))
                .ReturnsAsync((Sauce)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _sauceManagementService.EditSauceAsync(inputModel));
        }
    }
}