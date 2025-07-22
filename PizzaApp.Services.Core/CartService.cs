namespace PizzaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Models.MappingEntities;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Common.Dtos;
    using PizzaApp.Services.Core.Interfaces;
    using GCommon.Enums;

    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<User> _userManager;

        public CartService(ICartRepository cartRepository, UserManager<User> userManager)
        {
            this._cartRepository = cartRepository;
            this._userManager = userManager;
        }

        // TODO: refactor later 
        public async Task<bool> AddPizzaToCartAsync(PizzaCartDto pizzaDto, string userId)
        {
            Guid userIdGuid = Guid.Parse(userId);
            ShoppingCart cart = await this._cartRepository.GetCompleteCartByUserIdAsync(userIdGuid) ?? throw new Exception("Cart not found for user.");

            // A new pizza is created here because the user can modify the base pizza
            // and we want to keep the base pizza intact. Any modifications to the
            // pizza will damage data integrity
            // especially when checking order history. Maybe a better way to do this is to add 
            // a new db column called PizzaSnapshot where we store every ordered pizza regardless of whether
            // it is a base pizza or a user modified pizza.
            Pizza pizza = new()
            {
                Name = Guid.NewGuid().ToString(), // Name it a guid because the name will be taken from the base pizza
                DoughId = pizzaDto.DoughId,
                SauceId = pizzaDto.SauceId,
                CreatorUserId = userIdGuid,
                Toppings = pizzaDto.SelectedToppingsIds.Select(toppingId => new PizzaTopping
                {
                    ToppingId = toppingId
                }).ToList(),
                BasePizzaId = pizzaDto.PizzaId,
                PizzaType = PizzaType.OrderPizza // This is a pizza which is created for this order only,
                                                 // it will only appear in the cart and order history.
                                                 // Not sure if it's the best way to handle this.
            };

            // we don't care if the pizza already exists in
            // the cart because every ordered pizza is unique
            cart.Pizzas.Add(new ShoppingCartPizza
            {
                Pizza = pizza,
                ShoppingCartId = cart.Id
            });

            this._cartRepository.Update(cart);
            await this._cartRepository.SaveChangesAsync();

            return true;
        }
    }
}
