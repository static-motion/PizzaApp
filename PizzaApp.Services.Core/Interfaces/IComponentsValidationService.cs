namespace PizzaApp.Services.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface IComponentsValidationService
    {
        public Task ValidateComponentsExistAsync(int doughId, int? sauceId, IEnumerable<int> selectedToppings, bool ignoreFiltering = false);
    }
}
