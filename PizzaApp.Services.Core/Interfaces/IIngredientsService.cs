namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels;

    public interface IIngredientsService
    {
        Task<IReadOnlyList<ToppingCategoryViewWrapper>> GetAllCategoriesWithToppingsAsync(bool ignoreFiltering = false, bool disableTracking = false);
        Task<IReadOnlyList<DoughViewModel>> GetAllDoughsAsync(bool ignoreFiltering = false, bool disableTracking = false);
        Task<PizzaIngredientsViewWrapper> GetAllIngredientsAsync(bool ignoreFiltering = false, bool disaleTracking = false);
        Task<IReadOnlyList<SauceViewModel>> GetAllSaucesAsync(bool ignoreFiltering = false, bool disableTracking = false);
    }
}