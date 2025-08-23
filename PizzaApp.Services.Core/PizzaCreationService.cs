namespace PizzaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using PizzaApp.Data.Common.Exceptions;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.GCommon.Enums;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Admin;
    using PizzaApp.Web.ViewModels.Interfaces;

    public class PizzaCreationService : IPizzaCreationService
    {

        private readonly IPizzaRepository _pizzaRepository;
        private readonly UserManager<User> _userManager;
        private readonly IComponentsValidationService _componentsValidationService;

        public PizzaCreationService(IPizzaRepository pizzaRepository,
            UserManager<User> userManager,
            IComponentsValidationService componentsValidationService)
        {
            this._pizzaRepository = pizzaRepository;
            this._userManager = userManager;
            this._componentsValidationService = componentsValidationService;
        }

        public async Task CreatePizzaAsync(ICreatePizzaInputModel inputPizza, Guid userId)
        {
            User? user = await this._userManager.FindByIdAsync(userId.ToString())
                ?? throw new EntityNotFoundException(nameof(User), userId.ToString());

            bool isAdmin = await this._userManager.IsInRoleAsync(user, "Admin");

            if (inputPizza.PizzaType == PizzaType.AdminPizza && !isAdmin)
                throw new InvalidOperationException("Non admin users cannot create base pizzas.");

            try
            {
                await this._componentsValidationService.ValidateComponentsExistAsync
                    (inputPizza.DoughId, inputPizza.SauceId, inputPizza.SelectedToppingIds, isAdmin);
            }
            catch (EntityNotFoundException) { throw; }
            catch (EntityRangeCountMismatchException) { throw; }


            Pizza newPizza = MapPizzaProperties(inputPizza, userId);

            await this._pizzaRepository.AddAsync(newPizza);
            await this._pizzaRepository.SaveChangesAsync();
        }

        private static Pizza MapPizzaProperties(ICreatePizzaInputModel inputPizza, Guid userId)
        {
            Pizza pizza = new()
            {
                Name = inputPizza.Name,
                Description = inputPizza.Description,
                DoughId = inputPizza.DoughId,
                SauceId = inputPizza.SauceId,
                CreatorUserId = userId,
                PizzaType = inputPizza.PizzaType,
                Toppings = inputPizza.SelectedToppingIds.Select(id => new PizzaTopping()
                {
                    ToppingId = id,
                }).ToArray()
            };

            if (inputPizza is CreateBasePizzaInputModel basePizza)
            {
                pizza.ImageUrl = basePizza.ImageUrl;
                pizza.IsDeleted = basePizza.IsDeleted;
            }

            return pizza;
        }
    }
}

