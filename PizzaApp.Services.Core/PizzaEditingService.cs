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
    using PizzaApp.Web.ViewModels.MyPizzas;
    using System.Threading.Tasks;
    using static PizzaApp.Services.Common.ExceptionMessages;

    public class PizzaEditingService : IPizzaEditingService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly UserManager<User> _userManager;
        private readonly IComponentsValidationService componentsValidationService;

        public PizzaEditingService(
            IPizzaRepository pizzaRepository,
            UserManager<User> userManager, 
            IComponentsValidationService componentsValidationService)
        {
            this._pizzaRepository = pizzaRepository;
            this._userManager = userManager;
            this.componentsValidationService = componentsValidationService;
        }
        public async Task EditPizzaAsync(IEditPizzaInputModel inputPizza, Guid userId)
        {
            User? user = await this._userManager.FindByIdAsync(userId.ToString())
                ?? throw new EntityNotFoundException(userId.ToString());

            bool isAdmin = await this._userManager.IsInRoleAsync(user, "Admin");
            bool ignoreOwnership = false;

            if (inputPizza.PizzaType == PizzaType.AdminPizza)
            {
                if (!isAdmin)
                    throw new InvalidOperationException("Can't edit base pizza from a non-admin account.");
                
                this._pizzaRepository.IgnoreFiltering();
                // ownership is ignored ONLY if the pizzatype is basepizza AND the user is admin
                ignoreOwnership = true;
            }


            Pizza? pizzaToEdit = await this._pizzaRepository.GetByIdWithIngredientsAsync(inputPizza.Id)
                ?? throw new EntityNotFoundException(EntityNotFoundMessage);

            // if ignoreownership is true this means the user is an admin and
            // is trying to edit a base pizza. this is allowed.
            if (pizzaToEdit.CreatorUserId != user.Id && ignoreOwnership == false)
                throw new UserNotOwnerException();
                
            try
            {
                await this.componentsValidationService.ValidateComponentsExistAsync
                    (inputPizza.DoughId, inputPizza.SauceId, inputPizza.SelectedToppingIds, isAdmin);
            } 
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (EntityRangeCountMismatchException)
            {
                throw;
            }

            pizzaToEdit.Name = inputPizza.Name;
            pizzaToEdit.DoughId = inputPizza.DoughId;
            pizzaToEdit.SauceId = inputPizza.SauceId;
            pizzaToEdit.Description = inputPizza.Description;
            pizzaToEdit.IsDeleted = inputPizza.IsDeleted;
            pizzaToEdit.Toppings = inputPizza.SelectedToppingIds
                                        .Select(id => new PizzaTopping()
                                        {
                                            ToppingId = id
                                        })
                                        .ToList();

            if (inputPizza is EditBasePizzaInputModel basePizza)
            {
                pizzaToEdit.ImageUrl = basePizza.ImageUrl;
            }

            await this._pizzaRepository.SaveChangesAsync();
        }

        public async Task<EditCustomerPizzaInputModel> GetCustomerPizzaToEdit(int id, Guid userId)
        {
            Pizza? pizzaToEdit = await this._pizzaRepository
                .DisableTracking()
                .GetByIdWithIngredientsAsync(id)
                ?? throw new EntityNotFoundException(EntityNotFoundMessage);

            if (pizzaToEdit.PizzaType != PizzaType.CustomerPizza)
                throw new EntityNotFoundException($"No customer pizza with ID: {id} was found.");

            if (pizzaToEdit.CreatorUserId != userId)
                throw new UserNotOwnerException();

            return new EditCustomerPizzaInputModel()
            {
                Id = pizzaToEdit.Id,
                Name = pizzaToEdit.Name,
                Description = pizzaToEdit.Description,
                DoughId = pizzaToEdit.Dough.Id,
                SauceId = pizzaToEdit.SauceId,
                Price = pizzaToEdit.Dough.Price
                      + (pizzaToEdit.Sauce == null ? 0 : pizzaToEdit.Sauce.Price)
                      + pizzaToEdit.Toppings.Sum(t => t.Topping.Price),
                SelectedToppingIds = pizzaToEdit.Toppings.Select(t => t.ToppingId).ToHashSet(),
            };
        }

        public async Task<EditBasePizzaInputModel> GetBasePizzaToEdit(int id, Guid userId)
        {
            User? user = await this._userManager.FindByIdAsync(userId.ToString())
                ?? throw new EntityNotFoundException(userId.ToString());

            bool isAdmin = await this._userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin)
                throw new InvalidOperationException("Can't access base pizza from a non-admin account.");

            Pizza? pizza = await this._pizzaRepository
                .DisableTracking()
                .IgnoreFiltering()
                .GetByIdWithIngredientsAsync(id)
            ?? throw new EntityNotFoundException(id.ToString());

            if (pizza.PizzaType != PizzaType.AdminPizza)
                throw new InvalidOperationException($"No base pizza with ID: {id} was found.");

            return new EditBasePizzaInputModel
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Description = pizza.Description,
                DoughId = pizza.DoughId,
                SauceId = pizza.SauceId,
                ImageUrl = pizza.ImageUrl,
                Price = pizza.Dough.Price
                      + (pizza.Sauce == null ? 0 : pizza.Sauce.Price)
                      + pizza.Toppings.Sum(t => t.Topping.Price),
                SelectedToppingIds = pizza.Toppings.Select(t => t.ToppingId).ToHashSet(),
                IsDeleted = pizza.IsDeleted,
            };
        }
    }
}
