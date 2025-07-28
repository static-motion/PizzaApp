namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MenuManagementService : IMenuManagementService
    {
        private IPizzaRepository _pizzaRepository;
        private Dictionary<ManagementCategory, Func<Task<IEnumerable<ItemViewModel>>>> _categoryItemFetchers;

        public MenuManagementService(IPizzaRepository pizzaRepository)
        {
            this._pizzaRepository = pizzaRepository;
            _categoryItemFetchers = new()
            {
                [ManagementCategory.Pizzas] = this.GetAllPizzas,
            };
        }
        public async Task<IEnumerable<ItemViewModel>> GetAllItemsFromCategory(ManagementCategory category)
        {
            return await this._categoryItemFetchers[category]();
        }

        private async Task<IEnumerable<ItemViewModel>> GetAllPizzas()
        {
            IEnumerable<Pizza> pizzas = await this._pizzaRepository
                .GetAllAsync(asNoTracking: true, ignoreQueryFilters: true);

            return pizzas.Select(p => new ItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                IsActive = !p.IsDeleted
            });

        }
    }
}
