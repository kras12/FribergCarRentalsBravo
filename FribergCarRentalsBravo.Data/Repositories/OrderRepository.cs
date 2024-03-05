using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Helpers;
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
            if (!order.Car.IsActive)
            {
                throw new Exception("Can't create an order for an inactive car.");
            }

            applicationDbContext.Attach(order.Car);
            applicationDbContext.Attach(order.Customer);
            applicationDbContext.Add(order);
            await applicationDbContext.SaveChangesAsync();
            return PasswordHelper.RemovePassword(order)!;
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
            return PasswordHelper.RemovePassword(order)!;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return PasswordHelper.RemovePasswords(await applicationDbContext.Orders
                .Include(x => x.Customer).Include(x => x.Car).Include(x => x.Car.Category).Include(x => x.Car.Images).ToListAsync());            
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return PasswordHelper.RemovePassword(await applicationDbContext.Orders
                .Include(x => x.Customer).Include(x => x.Car).Include(x => x.Car.Category).Include(x => x.Car.Images)
                .FirstOrDefaultAsync(o => o.OrderId == id));
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

        public Task<int> GetAmountOfOrdersAsync()
        {
            return applicationDbContext.Orders.CountAsync();
        }

        /// <summary>
        /// Returns all orders having cars that are due to get picked up within the specified interval.
        /// </summary>
        /// <param name="startDate">The start of the date interval.</param>
        /// <param name="endDate">The end of the date interval.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> GetPendingPickups(DateTime startDate, DateTime endDate)
        {
            return PasswordHelper.RemovePasswords(await applicationDbContext.Orders
                .Where(x => x.PickupDate >= startDate && x.PickupDate <= endDate)
                .Include(x => x.Customer).Include(x => x.Car).Include(x => x.Car.Category).Include(x => x.Car.Images).ToListAsync());
        }

        public async Task<IEnumerable<Order>> GetAllTodaysPickupsAsync()
        {
            return PasswordHelper.RemovePasswords(await applicationDbContext.Orders
                .Where(x => x.PickupDate == DateTime.Today && x.IsCanceled == false)
                .Include(x => x.Customer).Include(x => x.Car).Include(x => x.Car.Category).Include(x => x.Car.Images).ToListAsync());
        }
    }
}
