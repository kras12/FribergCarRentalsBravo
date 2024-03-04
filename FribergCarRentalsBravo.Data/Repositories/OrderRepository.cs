﻿using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public OrderRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            applicationDbContext.Attach(order.Car);
            applicationDbContext.Attach(order.Customer);
            applicationDbContext.Add(order);
            await applicationDbContext.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderAsync(Order order)
        {
            applicationDbContext.Remove(order);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<Order> EditOrderAsync(Order order)
        {
            applicationDbContext.Update(order);
            await applicationDbContext.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await applicationDbContext.Orders.Include(x => x.Customer).Include(x => x.Car).Include(x => x.Car.Category).Include(x => x.Car.Images).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await applicationDbContext.Orders.Include(x => x.Customer).Include(x => x.Car).Include(x => x.Car.Category).Include(x => x.Car.Images).FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<bool> TryCancelOrderAsync(int id)
        {
            var order = await applicationDbContext.Orders.SingleOrDefaultAsync(x => x.OrderId == id);

            if (order is not null)
            {
                order.IsCanceled = true;
                await applicationDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<int> GetAmountOfOrdersAsync()
        {
            return applicationDbContext.Orders.Count();
        }

        /// <summary>
        /// Returns all orders having cars that are due to get picked up within the specified interval.
        /// </summary>
        /// <param name="startDate">The start of the date interval.</param>
        /// <param name="endDate">The end of the date interval.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetPendingPickups(DateTime startDate, DateTime endDate)
        {
            return await applicationDbContext.Orders.Where(x => x.PickupDate >= startDate && x.PickupDate <= endDate).Include(x => x.Customer).Include(x => x.Car).Include(x => x.Car.Category).Include(x => x.Car.Images).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllTodaysOrdersAsync()
        {
            return await applicationDbContext.Orders.Where(x => x.PickupDate == DateTime.Today && x.IsCanceled == false).Include(x => x.Customer).Include(x => x.Car).Include(x => x.Car.Category).Include(x => x.Car.Images).ToListAsync();
        }

    }
}
