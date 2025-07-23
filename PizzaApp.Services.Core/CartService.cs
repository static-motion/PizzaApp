namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Common.Serialization;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels;
    using PizzaApp.Web.ViewModels.ShoppingCart;
    using System.Collections.Generic;

    public class CartService : ICartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoughRepository _doughRepository;
        private readonly ISauceRepository _sauceRepository;
        private readonly IToppingCategoryRepository _toppingCategoryRepository;
        private readonly IPizzaRepository _pizzaRepository;

        public CartService(IUserRepository userRepository,
            IDoughRepository doughRepository, 
            ISauceRepository sauceRepository, 
            IToppingCategoryRepository toppingCategoryRepository,
            IPizzaRepository pizzaRepository)
        {
            this._userRepository = userRepository;
            this._doughRepository = doughRepository;
            this._sauceRepository = sauceRepository;
            this._toppingCategoryRepository = toppingCategoryRepository;
            this._pizzaRepository = pizzaRepository;
        }

        // TODO: refactor later 
        public async Task<bool> AddPizzaToCartAsync(PizzaCartDto pizzaDto, string userId)
        {
            Guid userIdGuid = Guid.Parse(userId);
            User? user = await this._userRepository.GetUserWithShoppingCartAsync(userIdGuid);

            if (user is null)
            {
                return false;
            }

            PizzaComponentsDto components = new()
            {
                DoughId = pizzaDto.DoughId,
                SauceId = pizzaDto.SauceId,
                SelectedToppings = pizzaDto.SelectedToppingsIds
            };

            ShoppingCartPizza pizza = new()
            {
                BasePizzaId = pizzaDto.PizzaId,
                Quantity = pizzaDto.Quantity,
                Price = 10m, // TODO: Calculate price based on components
                User = user,
                UserId = user.Id,
            };

            // serialize components to JSON
            pizza.SerializeComponentsToJson(components);

            // check if pizza already in cart
            ShoppingCartPizza? equalPizza = user.ShoppingCartPizzas.FirstOrDefault(p => 
                (p.GetComponentsFromJson()?.Equals(components) ?? false) // check if components match
                && pizza.BasePizzaId == p.BasePizzaId); // check if base pizza matches

            // if pizza with same components exists, increase quantity
            if (equalPizza is not null)
            {
                equalPizza.Quantity += pizzaDto.Quantity;
            }
            else // if not add new pizza to the cart
            {
                user.ShoppingCartPizzas.Add(pizza);
            }

            await this._userRepository.SaveChangesAsync();
            return true;
        }

        //TODO: Refactor to return all cart items instead of pizzas only.
        public async Task<IEnumerable<PizzaShoppingCartViewModel>> GetUserCart(Guid userId)
        {
            User? user = await this._userRepository.GetUserWithShoppingCartAsync(userId) 
                ?? throw new InvalidOperationException("User not found");

            IEnumerable<ToppingCategory> allToppingCategories = await this._toppingCategoryRepository
                .GetAllWithToppingsAsync(asNoTracking: true);

            List<PizzaShoppingCartViewModel> pizzasInCart = new List<PizzaShoppingCartViewModel>();

            foreach (ShoppingCartPizza pizza in user.ShoppingCartPizzas)
            {
                // load base pizza
                string? basePizzaName = (await this._pizzaRepository.GetByIdAsync(pizza.BasePizzaId))?.Name;

                PizzaComponentsDto components = 
                    pizza.GetComponentsFromJson() 
                    ?? throw new InvalidOperationException("Pizza components not found or invalid.");

                if (basePizzaName is null)
                {
                    throw new InvalidOperationException($"Base pizza with ID {pizza.BasePizzaId} not found.");
                }
                // load dough, sauce and toppings
                Dough dough = await this._doughRepository.GetByIdAsync(components.DoughId); //TODO: FIX

                Sauce? sauce = components.SauceId.HasValue ? 
                    await this._sauceRepository.GetByIdAsync(components.SauceId.Value)
                    : null;

                // this is stupid I hate the repository pattern
                PizzaShoppingCartViewModel pizzaViewModel = new()
                {
                    Name = basePizzaName,
                    DoughName = dough?.Type ?? "Unknown Dough",
                    SauceName = sauce?.Type,
                    Quantity = pizza.Quantity,
                    Price = pizza.Price
                };

                foreach (int toppingId in components.SelectedToppings)
                {
                    Topping? topping = allToppingCategories.SelectMany(tc => tc.Toppings)
                        .FirstOrDefault(t => t.Id == toppingId);
                    if (topping is null)
                    {
                        continue;
                    }

                    string categoryName = topping.ToppingCategory.Name;

                    if (!pizzaViewModel.Toppings.ContainsKey(categoryName))
                    {
                        pizzaViewModel.Toppings.Add(categoryName, new List<ToppingViewModel>());
                    }

                    pizzaViewModel.Toppings[categoryName].Add(new ToppingViewModel()
                    {
                        Id = topping.Id,
                        Name = topping.Name,
                        Price = topping.Price
                    });
                }

                pizzasInCart.Add(pizzaViewModel);
            }

            return pizzasInCart;
        }
    }
}
