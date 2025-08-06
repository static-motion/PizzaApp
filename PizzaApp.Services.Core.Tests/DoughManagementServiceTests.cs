namespace PizzaApp.Services.Core.Tests
{
    using Moq;
    using NUnit.Framework;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core;
    using PizzaApp.Web.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class DoughManagementServiceTests
    {
        private Mock<IDoughRepository> _doughRepositoryMock;
        private DoughManagementService _doughManagementService;

        [SetUp]
        public void Setup()
        {
            _doughRepositoryMock = new Mock<IDoughRepository>();
            _doughManagementService = new DoughManagementService(_doughRepositoryMock.Object);
        }

        [Test]
        public async Task GetDoughsOverviewPaginatedAsync_ReturnsCorrectPaginationData()
        {
            // Arrange
            var page = 2;
            var pageSize = 5;
            var totalItems = 15;
            var doughs = new List<Dough>
            {
                new Dough { Id = 6, Type = "Thin Crust", IsDeleted = false },
                new Dough { Id = 7, Type = "Thick Crust", IsDeleted = true },
                new Dough { Id = 8, Type = "Gluten Free", IsDeleted = false }
            };

            _doughRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(totalItems);

            _doughRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().TakeAsync((page - 1) * pageSize, pageSize))
                .ReturnsAsync(doughs);

            // Act
            var result = await _doughManagementService.GetDoughsOverviewPaginatedAsync(page, pageSize);

            // Assert

            var firstItem = result.Item2.First();
            Assert.Multiple(() =>
            {
                Assert.That(result.Item1, Is.EqualTo(totalItems));
                Assert.That(result.Item2.Count(), Is.EqualTo(3));
                Assert.That(firstItem.Id, Is.EqualTo(doughs[0].Id));
                Assert.That(firstItem.Name, Is.EqualTo(doughs[0].Type));
                Assert.That(firstItem.IsActive, Is.EqualTo(!doughs[0].IsDeleted));
            });

            _doughRepositoryMock.Verify(x => x.TotalEntityCountAsync(), Times.Once);
            _doughRepositoryMock.Verify(x => x.DisableTracking().IgnoreFiltering().TakeAsync((page - 1) * pageSize, pageSize), Times.Once);
        }

        [Test]
        public async Task GetDoughsOverviewPaginatedAsync_WithEmptyRepository_ReturnsZeroCountAndEmptyList()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;

            _doughRepositoryMock.Setup(x => x.TotalEntityCountAsync())
                .ReturnsAsync(0);

            _doughRepositoryMock.Setup(x => x.DisableTracking().IgnoreFiltering().TakeAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Dough>());

            // Act
            var result = await _doughManagementService.GetDoughsOverviewPaginatedAsync(page, pageSize);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Item1, Is.EqualTo(0));
                Assert.That(result.Item2, Is.Empty);
            });
        }

        [Test]
        public async Task GetDoughDetailsByIdAsync_WithValidId_ReturnsCorrectDetails()
        {
            // Arrange
            var doughId = 1;
            var dough = new Dough
            {
                Id = doughId,
                Type = "Thin Crust",
                Description = "Crispy thin crust",
                Price = 2.50m,
                IsDeleted = false
            };

            _doughRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(doughId))
                .ReturnsAsync(dough);

            // Act
            var result = await _doughManagementService.GetDoughDetailsByIdAsync(doughId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(dough.Id));
                Assert.That(result.Type, Is.EqualTo(dough.Type));
                Assert.That(result.Description, Is.EqualTo(dough.Description));
                Assert.That(result.Price, Is.EqualTo(dough.Price));
                Assert.That(result.IsDeleted, Is.EqualTo(dough.IsDeleted));
            });

            _doughRepositoryMock.Verify(x => x.IgnoreFiltering().GetByIdAsync(doughId), Times.Once);
        }

        [Test]
        public void GetDoughDetailsByIdAsync_WithInvalidId_ThrowsInvalidOperationException()
        {
            // Arrange
            var invalidId = 999;
            _doughRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(invalidId))
                .ReturnsAsync((Dough)null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doughManagementService.GetDoughDetailsByIdAsync(invalidId));
        }

        [Test]
        public async Task EditDoughAsync_WithValidModel_UpdatesDoughCorrectly()
        {
            // Arrange
            var model = new EditDoughInputModel
            {
                Id = 1,
                Type = "New Type",
                Description = "New Description",
                Price = 3.00m,
                IsDeleted = true
            };

            var existingDough = new Dough
            {
                Id = 1,
                Type = "Old Type",
                Description = "Old Description",
                Price = 2.50m,
                IsDeleted = false
            };

            _doughRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(model.Id))
                .ReturnsAsync(existingDough);

            // Act
            await _doughManagementService.EditDoughAsync(model);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(existingDough.Type, Is.EqualTo(model.Type));
                Assert.That(existingDough.Description, Is.EqualTo(model.Description));
                Assert.That(existingDough.Price, Is.EqualTo(model.Price));
                Assert.That(existingDough.IsDeleted, Is.EqualTo(model.IsDeleted));
            });

            _doughRepositoryMock.Verify(x => x.Update(existingDough), Times.Once);
            _doughRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void EditDoughAsync_WithInvalidId_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new EditDoughInputModel { Id = 999, Type = "invalid", Description = "lol" };
            _doughRepositoryMock.Setup(x => x.IgnoreFiltering().GetByIdAsync(model.Id))
                .ReturnsAsync((Dough)null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doughManagementService.EditDoughAsync(model));
        }
    }
}