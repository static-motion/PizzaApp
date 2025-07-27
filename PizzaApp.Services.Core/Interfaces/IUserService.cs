namespace PizzaApp.Services.Core.Interfaces
{
    using PizzaApp.Web.ViewModels.Address;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task AddAddressAsync(Guid userId, AddressInputModel input);
        Task DeleteAddressAsync(Guid userId, int id);
        Task<IEnumerable<AddressViewModel>> GetUserAddressesAsync(Guid userId);
    }
}
