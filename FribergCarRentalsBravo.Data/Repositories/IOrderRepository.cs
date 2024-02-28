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
        /// <summary>
        /// Adds an order to the database.
        /// </summary>
        /// <param name="order">The order to add.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task AddAsync(Order order);
    }
}
