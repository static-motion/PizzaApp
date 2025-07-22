namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Services.Common.Dtos;
    using System.Threading.Tasks;

    public interface ICartService
    {
        Task<bool> AddPizzaToCartAsync(PizzaCartDto pizzaDto, string userId);
    }
}
