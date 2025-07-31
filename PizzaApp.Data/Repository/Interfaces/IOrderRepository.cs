namespace PizzaApp.Data.Repository.Interfaces
{
    using PizzaApp.Data.Models;
    using System;

    public interface IOrderRepository : IRepository<Order, Guid, OrderRepository>
    {
        public Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
    }
}
