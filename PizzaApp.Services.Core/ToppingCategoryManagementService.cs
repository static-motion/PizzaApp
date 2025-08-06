namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class ToppingCategoryManagementService : IToppingCategoryManagementService
    {
        private readonly IToppingCategoryRepository _toppingCategoryRepository;

        public ToppingCategoryManagementService(IToppingCategoryRepository toppingCategoryRepository)
        {
            _toppingCategoryRepository = toppingCategoryRepository;
        }

        public async Task<(int, IEnumerable<MenuItemViewModel>)> GetToppingCategoriesOverviewPaginatedAsync(int page, int pageSize)
        {
            int totalItems = await _toppingCategoryRepository.TotalEntityCountAsync();

            IEnumerable<ToppingCategory> pagedToppingCategories = await _toppingCategoryRepository
                .DisableTracking()
                .IgnoreFiltering()
                .TakeAsync(skip: (page - 1) * pageSize, take: pageSize);

            return (totalItems, pagedToppingCategories.Select(d => new MenuItemViewModel
            {
                Id = d.Id,
                IsActive = !d.IsDeleted,
                Name = d.Name
            }));
        }

        public async Task<EditToppingCategoryInputModel> GetToppingCategoryDetailsByIdAsync(int id)
        {
            ToppingCategory? category = await _toppingCategoryRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetByIdAsync(id)
            ?? throw new InvalidOperationException("Topping category not found.");

            return new EditToppingCategoryInputModel
            {
                Id = category.Id,
                Name = category.Name,
                IsDeleted = category.IsDeleted,
            };
        }

        public async Task EditToppingCategoryAsync(EditToppingCategoryInputModel inputModel)
        {
            ToppingCategory? toppingCategory = await _toppingCategoryRepository
                .IgnoreFiltering()
                .GetByIdAsync(inputModel.Id)
            ?? throw new InvalidOperationException("Topping category not found.");

            toppingCategory.Name = inputModel.Name;
            toppingCategory.IsDeleted = inputModel.IsDeleted;

            _toppingCategoryRepository.Update(toppingCategory);
            await _toppingCategoryRepository.SaveChangesAsync();
        }
    }
}
