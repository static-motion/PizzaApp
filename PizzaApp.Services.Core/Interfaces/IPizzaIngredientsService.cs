namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels;

    public interface IPizzaIngredientsService
    {
        Task<IReadOnlyList<ToppingCategoryViewModel>> GetAllCategoriesWithToppingsAsync();
        Task<IReadOnlyList<DoughViewModel>> GetAllDoughsAsync();
        Task<IReadOnlyList<SauceViewModel>> GetAllSaucesAsync();
    }
}