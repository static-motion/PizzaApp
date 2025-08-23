namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class DoughManagementService : IDoughManagementService
    {
        private readonly IDoughRepository _doughRepository;

        public DoughManagementService(IDoughRepository doughRepository)
        {
            _doughRepository = doughRepository;
        }

        public async Task<(int, IEnumerable<MenuItemViewModel>)> GetDoughsOverviewPaginatedAsync(int page, int pageSize)
        {
            int totalItems = await _doughRepository.TotalEntityCountAsync();

            IEnumerable<Dough> doughs = await _doughRepository
                .DisableTracking()
                .IgnoreFiltering()
                .TakeAsync(skip: (page - 1) * pageSize, take: pageSize);

            return (totalItems, doughs.Select(d => new MenuItemViewModel
            {
                Id = d.Id,
                Name = d.Type,
                IsActive = d.IsDeleted == false,
            }));
        }

        public async Task<EditDoughInputModel> GetDoughDetailsByIdAsync(int id)
        {
            Dough? dough = await _doughRepository
                .IgnoreFiltering()
                .GetByIdAsync(id)
            ?? throw new EntityNotFoundException(nameof(Dough), id.ToString());

            return new EditDoughInputModel
            {
                Id = dough.Id,
                Description = dough.Description,
                Price = dough.Price,
                Type = dough.Type,
                IsDeleted = dough.IsDeleted
            };
        }

        public async Task EditDoughAsync(EditDoughInputModel model)
        {
            Dough? dough = await _doughRepository
                .IgnoreFiltering()
                .GetByIdAsync(model.Id)
            ?? throw new EntityNotFoundException(nameof(Dough), model.Id.ToString());

            dough.Type = model.Type;
            dough.Price = model.Price;
            dough.Description = model.Description;
            dough.IsDeleted = model.IsDeleted;

            _doughRepository.Update(dough);
            await _doughRepository.SaveChangesAsync();
        }
    }
}
