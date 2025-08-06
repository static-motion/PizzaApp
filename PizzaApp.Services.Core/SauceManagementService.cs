namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class SauceManagementService : ISauceManagementService
    {
        private readonly ISauceRepository _sauceRepository;

        public SauceManagementService(ISauceRepository sauceRepository)
        {
            _sauceRepository = sauceRepository;
        }

        public async Task<(int, IEnumerable<MenuItemViewModel>)> GetSaucesOverviewPaginatedAsync(int page, int pageSize)
        {
            int saucesCount = await _sauceRepository.TotalEntityCountAsync();

            IEnumerable<Sauce> sauces = await _sauceRepository
                .DisableTracking()
                .IgnoreFiltering()
                .TakeAsync(skip: (page - 1) * pageSize, take: pageSize);

            return (saucesCount, sauces.Select(d => new MenuItemViewModel
            {
                Id = d.Id,
                IsActive = d.IsDeleted == false,
                Name = d.Type
            }));
        }

        public async Task<EditSauceInputModel> GetSauceDetailsByIdAsync(int id)
        {
            Sauce? sauce = await _sauceRepository
                .IgnoreFiltering()
                .DisableTracking()
                .GetByIdAsync(id)
            ?? throw new InvalidOperationException("Sauce not found.");

            return new EditSauceInputModel
            {
                Id = sauce.Id,
                Description = sauce.Description,
                IsDeleted = sauce.IsDeleted,
                Type = sauce.Type,
                Price = sauce.Price
            };
        }

        public async Task EditSauceAsync(EditSauceInputModel inputSauce)
        {
            Sauce? sauce = await _sauceRepository
                .IgnoreFiltering()
                .GetByIdAsync(inputSauce.Id)
            ?? throw new InvalidOperationException("Sauce not found.");

            sauce.Type = inputSauce.Type;
            sauce.Price = inputSauce.Price;
            sauce.Description = inputSauce.Description;
            sauce.IsDeleted = inputSauce.IsDeleted;

            _sauceRepository.Update(sauce);
            await _sauceRepository.SaveChangesAsync();
        }
    }
}

