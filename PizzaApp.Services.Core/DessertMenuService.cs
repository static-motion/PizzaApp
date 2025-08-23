namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Menu;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DessertMenuService : IDessertMenuService
    {
        private readonly IDessertRepository _dessertRepository;

        public DessertMenuService(IDessertRepository dessertRepository)
        {
            this._dessertRepository = dessertRepository;
        }

        public async Task<IEnumerable<MenuItemViewModel>> GetAllBaseItemsAsync()
        {
            IEnumerable<Dessert> allDesserts = await this._dessertRepository.DisableTracking().GetAllAsync();

            return allDesserts
                .Select(d => new MenuItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ImageUrl = d.ImageUrl,
                    Category = MenuCategory.Desserts
                });
        }

        public async Task<MenuItemDetailsViewModel> GetDetailsById(int id)
        {
            Dessert dessert = await this._dessertRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Dessert), id.ToString());

            return new MenuItemDetailsViewModel()
            {
                Id = dessert.Id,
                Name = dessert.Name,
                Category = MenuCategory.Desserts,
                Description = dessert.Description,
                ImageUrl = dessert.ImageUrl,
                Price = dessert.Price
            };
        }
    }
}
