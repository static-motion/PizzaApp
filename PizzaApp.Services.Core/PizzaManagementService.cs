namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Common.Exceptions;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;

    public class PizzaManagementService : IPizzaManagementService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IPizzaCreationService _pizzaCreationService;
        private readonly IPizzaEditingService _pizzaEditingService;
        private readonly IPizzaActiveStatusService _pizzaActiveStatusService;

        public PizzaManagementService(
            IPizzaRepository pizzaRepository,
            IPizzaCreationService pizzaService,
            IPizzaEditingService pizzaEditingService,
            IPizzaActiveStatusService pizzaActiveStatusService)
        {
            this._pizzaRepository = pizzaRepository;
            this._pizzaCreationService = pizzaService;
            this._pizzaEditingService = pizzaEditingService;
            this._pizzaActiveStatusService = pizzaActiveStatusService;
        }

        public async Task<(int, IEnumerable<MenuItemViewModel>)> GetPizzasOverviewPaginatedAsync(int page, int pageSize)
        {
            int pizzasCount = await _pizzaRepository.TotalEntityCountAsync();

            IEnumerable<Pizza> pizzas = await _pizzaRepository
                .DisableTracking()
                .IgnoreFiltering()
                .TakeBasePizzasWithIngredientsAsync(skip: (page - 1) * pageSize, take: pageSize);

            return (pizzasCount, pizzas.Select(p => new MenuItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                IsActive = this._pizzaActiveStatusService.IsPizzaActive(p)
            }));
        }
        public async Task<EditBasePizzaInputModel> GetBasePizzaAsync(int id, Guid userId)
        {
            try
            {
                EditBasePizzaInputModel pizzaDetails
                                = await this._pizzaEditingService.GetBasePizzaToEdit(id, userId);
                return pizzaDetails;
            }
            catch (Exception) { throw; } // TODO: 
        }

        public async Task EditPizzaAsync(EditBasePizzaInputModel inputModel, Guid userId)
        {
            try
            {
                await this._pizzaEditingService.EditPizzaAsync(inputModel, userId);
            } 
            catch (Exception) { throw; } // TODO: //
        }

        public async Task CreateBasePizzaAsync(CreateBasePizzaInputModel inputPizza, Guid userId)
        {
            try
            {
                await this._pizzaCreationService.CreatePizzaAsync(inputPizza, userId);
            }
            catch (EntityRangeCountMismatchException)
            {
                throw;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
        }
    }
}
