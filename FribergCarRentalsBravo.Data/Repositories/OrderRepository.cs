using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using FribergCarRentalsBravo.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        #region Fields

        private readonly ApplicationDbContext applicationDbContext;

        #endregion

        #region Constructors

        public OrderRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an order to the database.
        /// </summary>
        /// <param name="order">The order to add.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task AddAsync(Order order)
        {
            await applicationDbContext.Orders.AddAsync(order);
            await applicationDbContext.SaveChangesAsync();
        }

        #endregion
    }
}
