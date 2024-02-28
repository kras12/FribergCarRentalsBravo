using FribergCarRentalsBravo.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order Order);
        Task DeleteOrderAsync(Order order);
        Task<Order> EditOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
    }
}
