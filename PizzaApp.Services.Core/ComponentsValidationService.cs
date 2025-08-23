namespace PizzaApp.Services.Core
{
    using PizzaApp.Data.Common.Exceptions;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Common.Exceptions;
    using PizzaApp.Services.Core.Interfaces;
    using System.Threading.Tasks;

    public class ComponentsValidationService : IComponentsValidationService
    {
        private readonly IDoughRepository _doughRepository;
        private readonly ISauceRepository _sauceRepository;
        private readonly IToppingRepository _toppingRepository;

        public ComponentsValidationService(IDoughRepository doughRepository, 
            ISauceRepository sauceRepository, 
            IToppingRepository toppingRepository) 
        {
            this._doughRepository = doughRepository;
            this._sauceRepository = sauceRepository;
            this._toppingRepository = toppingRepository;
        }
        public async Task ValidateComponentsExistAsync(int doughId, int? sauceId, IEnumerable<int> selectedToppings, bool ignoreFiltering = false)
        {
            // three separate ignorefiltering checks because the flags won't get reset if 
            // an exception is thrown before a query is exectued :/
            if (ignoreFiltering)
            {
                this._doughRepository.IgnoreFiltering();
            }
            bool doughExists = await this._doughRepository.ExistsAsync(d => d.Id == doughId);
            if (!doughExists) throw new EntityNotFoundException(nameof(Dough), doughId.ToString());

            if (sauceId.HasValue)
            {
                if (ignoreFiltering)
                {
                    this._sauceRepository.IgnoreFiltering();
                }
                var sauceExists = await this._sauceRepository.ExistsAsync(s => sauceId.Value == s.Id);
                if (!sauceExists) throw new EntityNotFoundException(nameof(Sauce), sauceId.Value.ToString());
            }

            try
            {
                if (ignoreFiltering)
                {
                    this._toppingRepository.IgnoreFiltering();
                }
                var validToppings = await this._toppingRepository.GetRangeByIdsAsync(selectedToppings);
            }
            catch (EntityRangeCountMismatchException)
            {
                throw;
            }
        }
    }
}
