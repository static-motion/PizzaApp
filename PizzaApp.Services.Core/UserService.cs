namespace PizzaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using PizzaApp.Data.Models;
    using PizzaApp.Data.Repository.Interfaces;
    using PizzaApp.Services.Core.Interfaces;
    using PizzaApp.Web.ViewModels.Address;

    using static PizzaApp.Services.Common.ExceptionMessages;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository, UserManager<User> userManager) 
        {
            this._userRepository = userRepository;
            this._userManager = userManager;
        }

        public async Task AddAddressAsync(Guid userId, AddressInputModel input)
        {
            User? user = await this._userRepository.GetByIdAsync(userId) 
                ?? throw new ArgumentException("User not found");

            user.Addresses.Add(new Address
            {
                City = input.City,
                AddressLine1 = input.AddressLine1,
                AddressLine2 = input.AddressLine2
            });

            await this._userRepository.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(Guid userId, int id)
        {
            User? user = await this._userRepository.GetUserWithAddressesAsync(userId)
                ?? throw new ArgumentException(AddressNotFound);

            Address? address = user.Addresses.FirstOrDefault(a => a.Id == id)
                ?? throw new ArgumentException(AddressNotFound);

            address.IsDeleted = true;
            await this._userRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<AddressViewModel>> GetUserAddressesAsync(Guid userId)
        {
            User? user = await this._userRepository.GetUserWithAddressesAsync(userId);

            if (user is null)
            {
                return [];
            }

            return user.Addresses.Select(a => new AddressViewModel
            {
                Id = a.Id,
                City = a.City,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2
            });
        }
    }
}
