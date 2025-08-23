using PizzaApp.Data.Common.Exceptions;
using PizzaApp.Data.Common.Serialization;
using PizzaApp.Data.Models;
using PizzaApp.Data.Repository;
using PizzaApp.Data.Repository.Interfaces;
using PizzaApp.Services.Common.Dtos;
using PizzaApp.Services.Common.Exceptions;
using PizzaApp.Services.Core.Interfaces;
using PizzaApp.Web.ViewModels;
using PizzaApp.Web.ViewModels.ShoppingCart;
using System.Text.Json;

public class PizzaCartService : IPizzaCartService
{
    private readonly IUserRepository _userRepository;
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IDoughRepository _doughRepository;
    private readonly ISauceRepository _sauceRepository;
    private readonly IToppingCategoryRepository _toppingCategoryRepository;
    private readonly IComponentsValidationService _componentsValidationService;

    public PizzaCartService(
        IUserRepository userRepository,
        IPizzaRepository pizzaRepository,
        IDoughRepository doughRepository,
        ISauceRepository sauceRepository,
        IToppingCategoryRepository toppingCategoryRepository,
        IComponentsValidationService componentsValidationService)
    {
        _userRepository = userRepository;
        _pizzaRepository = pizzaRepository;
        _doughRepository = doughRepository;
        _sauceRepository = sauceRepository;
        _toppingCategoryRepository = toppingCategoryRepository;
        _componentsValidationService = componentsValidationService;
    }

    public async Task AddPizzaToCartAsync(PizzaCartDto pizzaDto, Guid userId)
    {
        User? user = await _userRepository.GetUserWithShoppingCartAsync(userId)
            ?? throw new EntityNotFoundException(nameof(User), userId.ToString());

        PizzaComponentsDto components = new()
        {
            DoughId = pizzaDto.DoughId,
            SauceId = pizzaDto.SauceId,
            SelectedToppings = pizzaDto.SelectedToppingsIds
        };

        string componentsJson = JsonSerializer.Serialize(components);

        // Validate components exist before adding to cart
        try
        {
            int doughId = components.DoughId;
            int? sauceId = components.SauceId;
            IEnumerable<int> selectedToppings = components.SelectedToppings;
            await this._componentsValidationService.ValidateComponentsExistAsync(doughId, sauceId, selectedToppings);
        }
        catch (EntityNotFoundException) { throw; }
        catch (EntityRangeCountMismatchException) { throw; }

        ShoppingCartPizza? existingPizza = user.ShoppingCartPizzas.FirstOrDefault(p =>
            p.BasePizzaId == pizzaDto.PizzaId &&
            (p.PizzaComponentsJson?.Equals(componentsJson) ?? false));

        if (existingPizza != null)
        {
            existingPizza.Quantity += pizzaDto.Quantity;
        }
        else
        {
            ShoppingCartPizza pizza = new ShoppingCartPizza
            {
                BasePizzaId = pizzaDto.PizzaId,
                Quantity = pizzaDto.Quantity,
                UserId = user.Id,
            };

            pizza.SerializeComponentsToJson(components);
            user.ShoppingCartPizzas.Add(pizza);
        }
        await _userRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<CartPizzaViewModel>> GetPizzasInCart(User user)
    {
        IEnumerable<int> pizzaIds = user.ShoppingCartPizzas.Select(p => p.BasePizzaId).Distinct();
        IEnumerable<Pizza> basePizzas = await _pizzaRepository.IgnoreFiltering().DisableTracking().GetRangeByIdsAsync(pizzaIds);
        Dictionary<int, Pizza> basePizzaDict = basePizzas.ToDictionary(p => p.Id);

        Dictionary<int, Dough> allDoughs = await this._doughRepository.IgnoreFiltering().DisableTracking().GetLookup();

        Dictionary<int, Sauce> allSauces = await _sauceRepository.DisableTracking().IgnoreFiltering().GetLookup();

        IEnumerable<ToppingCategory> allToppings = await _toppingCategoryRepository.GetAllCategoriesWithToppingsAsync();
        Dictionary<int, Topping> toppingDict = allToppings.SelectMany(tc => tc.Toppings).ToDictionary(t => t.Id);

        List<ShoppingCartPizza> validPizzas = new();
        List<CartPizzaViewModel> result = new();

        foreach (ShoppingCartPizza cartPizza in user.ShoppingCartPizzas)
        {
            CartPizzaViewModel? model = this.MapToViewModel(cartPizza, basePizzaDict, allDoughs, allSauces, toppingDict);

            if (model is null)
                continue;
            
            validPizzas.Add(cartPizza);
            result.Add(model);
        }

        if (validPizzas.Count != user.ShoppingCartPizzas.Count)
        {
            user.ShoppingCartPizzas.Clear();
            user.ShoppingCartPizzas = validPizzas;
            await this._userRepository.SaveChangesAsync();
        }

        return result;
        

    }

    private CartPizzaViewModel? MapToViewModel(
        ShoppingCartPizza pizza,
        Dictionary<int, Pizza> basePizzaDict,
        Dictionary<int, Dough> doughDict,
        Dictionary<int, Sauce> sauceDict,
        Dictionary<int, Topping> toppingDict)
    {
        try
        {
            if (!basePizzaDict.TryGetValue(pizza.BasePizzaId, out var basePizza) || basePizza.IsDeleted)
                throw new EntityNotFoundException(nameof(Pizza), pizza.BasePizzaId.ToString());

            PizzaComponentsDto? components = pizza.GetComponentsFromJson()
                ?? throw new InvalidOperationException();

            if (!doughDict.TryGetValue(components.DoughId, out var dough) || dough.IsDeleted)
                throw new EntityNotFoundException(nameof(Dough), components.DoughId.ToString());

            Sauce? sauce = null;
            // this is done because selecting a sauce for the pizza is not mandatory.
            // we need to differenciate between when a user has not picked a sauce and
            // when the sauce has been disabled (IsDeleted = true)
            // if SauceId doesn't have a value we can conclude that the user has not picked a sauce.
            if (components.SauceId.HasValue) // if the SauceId has value
            {                                // we try to get it from the lookup dictionary
                if (!sauceDict.TryGetValue(components.SauceId.Value, out sauce))
                {
                    throw new EntityNotFoundException(nameof(Sauce), components.SauceId.Value.ToString());// if we end up here this means
                             // that the sauce has been filtered out by the query filter
                             // due to IsDeleted = true. We skip building the cart pizza
                             // with the inactive sauce
                }
            }

            var pizzaVm = new CartPizzaViewModel
            {
                Id = pizza.Id,
                Name = basePizza.Name,
                DoughName = dough.Type,
                SauceName = sauce?.Type ?? "No sauce",
                Quantity = pizza.Quantity,
                Toppings = new Dictionary<string, List<ToppingViewModel>>()
            };

            // Process toppings with validation
            foreach (var toppingId in components.SelectedToppings)
            {
                if (!toppingDict.TryGetValue(toppingId, out var topping))
                    throw new EntityNotFoundException(nameof(Topping), toppingId.ToString());

                var categoryName = topping.ToppingCategory.Name;

                if (!pizzaVm.Toppings.TryGetValue(categoryName, out var categoryItems))
                {
                    categoryItems = new List<ToppingViewModel>();
                    pizzaVm.Toppings.Add(categoryName, categoryItems);
                }

                categoryItems.Add(new ToppingViewModel
                {
                    Id = topping.Id,
                    Name = topping.Name,
                    Price = topping.Price
                });
            }

            pizzaVm.Price = this.CalculatePizzaPrice(pizzaVm, dough, sauce);
            return pizzaVm;
        }
        catch (Exception ex)
        {
            return null; // Skip invalid pizzas
        }
    }

    private decimal CalculatePizzaPrice(CartPizzaViewModel pizzaVm, Dough dough, Sauce? sauce)
    {
        decimal toppingsTotalPrice = pizzaVm.Toppings.Values
                        .SelectMany(t => t)
                        .Sum(t => t.Price);

        return toppingsTotalPrice + dough.Price + (sauce?.Price ?? 0);
    }
}