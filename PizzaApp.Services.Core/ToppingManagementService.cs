namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ToppingManagementService : IToppingManagementService
    {
        private readonly IToppingRepository _toppingRepository;
        private readonly IToppingCategoryRepository _toppingCategoryRepository;

        public ToppingManagementService(
            IToppingRepository toppingRepository,
            IToppingCategoryRepository toppingCategoryRepository)
        {
            _toppingRepository = toppingRepository;
            _toppingCategoryRepository = toppingCategoryRepository;
        }

        public async Task<(int, IEnumerable<MenuItemViewModel>)> GetToppingsOverviewPaginatedAsync(int page, int pageSize)
        {
            int totalItems = await _toppingRepository.TotalEntityCountAsync();

            IEnumerable<Topping> toppings = await _toppingRepository
                .DisableTracking()
                .IgnoreFiltering()
                .TakeWithCategoriesAsync(skip: (page - 1) * pageSize, take: pageSize);

            return (totalItems, toppings.Select(t => new MenuItemViewModel
            {
                Id = t.Id,
                IsActive = t.IsDeleted == false && t.ToppingCategory.IsDeleted == false,
                Name = t.Name
            }));
        }

        public async Task<EditToppingViewWrapper> GetToppingDetailsByIdAsync(int id)
        {
            Topping? topping = await _toppingRepository
                .IgnoreFiltering()
                .DisableTracking()
                .GetByIdAsync(id)
            ?? throw new InvalidOperationException("Topping not found.");

            IEnumerable<ToppingCategory> toppingCategories = await _toppingCategoryRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetAllAsync();

            return new EditToppingViewWrapper
            {
                AllCategories = toppingCategories.Select(tc => new ToppingCategorySelectViewModel
                {
                    Id = tc.Id,
                    Name = tc.Name,
                    IsActive = tc.IsDeleted == false
                }),
                ToppingInputModel = new EditToppingInputModel
                {
                    Id = topping.Id,
                    Description = topping.Description,
                    Name = topping.Name,
                    ToppingCategoryId = topping.ToppingCategoryId,
                    IsDeleted = topping.IsDeleted,
                    Price = topping.Price
                }
            };
        }

        public async Task EditToppingAsync(EditToppingInputModel inputTopping)
        {
            Topping? topping = await _toppingRepository
                .IgnoreFiltering()
                .GetByIdAsync(inputTopping.Id)
            ?? throw new InvalidOperationException("Topping not found.");

            bool categoryExists = await _toppingCategoryRepository
                .IgnoreFiltering()
                .ExistsAsync(tc => tc.Id == inputTopping.ToppingCategoryId);

            if (!categoryExists)
                throw new InvalidOperationException("Topping category does not exist.");

            topping.Name = inputTopping.Name;
            topping.Description = inputTopping.Description;
            topping.IsDeleted = inputTopping.IsDeleted;
            topping.Price = inputTopping.Price;
            topping.ToppingCategoryId = inputTopping.ToppingCategoryId;

            _toppingRepository.Update(topping);
            await _toppingRepository.SaveChangesAsync();
        }
    }
}
