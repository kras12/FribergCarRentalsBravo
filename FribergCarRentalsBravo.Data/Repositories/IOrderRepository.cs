
﻿using FribergCarRentalsBravo.DataAccess.Entities;
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
        Task<Order?> GetOrderByIdAsync(int id);
        public Task<bool> TryCancelOrderAsync(int id);
        Task<int> GetAmountOfOrdersAsync();

        /// <summary>
        /// Returns all orders having cars that are due to get picked up within the specified interval.
        /// </summary>
        /// <param name="startDate">The start of the date interval.</param>
        /// <param name="endDate">The end of the date interval.</param>
        /// <returns></returns>
        public Task<IEnumerable<Order>> GetPendingPickups(DateTime startDate, DateTime endDate);

        public Task<IEnumerable<Order>> GetAllTodaysPickupsAsync();
        public Task<List<Order>> GetCustomerOrdersAsync(int customerId);
    }
}
