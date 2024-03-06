using FribergCarRentalsBravo.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Helpers
{
    /// <summary>
    /// A helper class to manage password related tasks. 
    /// </summary>
    static internal class PasswordHelper
    {
        #region Methods

        /// <summary>
        /// Attempts to removes the password from an customer entity if it exists. 
        /// The entity is returned or null if it didn't exist. 
        /// </summary>
        /// <param name="customer">The entity to modify.</param>
        /// <returns>The modified <see cref="Customer"/> object or null if there was no entity.</returns>
        static public Customer? RemovePassword(Customer? customer)
        {
            if (customer is not null)
            {
                RemovePasswords(new List<Customer> { customer });
            }

            return customer;
        }

        /// <summary>
        /// Attempts to removes the password from an admin entity if it exists. 
        /// The entity is returned or null if it didn't exist. 
        /// </summary>
        /// <param name="admin">The entity to modify.</param>
        /// <returns>The modified <see cref="AdminUser"/> object or null if there was no entity.</returns>
        static public AdminUser? RemovePassword(AdminUser? admin)
        {
            if (admin is not null)
            {
                admin.Password = "";
            }

            return admin;
        }

        /// <summary>
        /// Attempts to removes the password from an order entity if it exists. 
        /// The entity is returned or null if it didn't exist. 
        /// </summary>
        /// <param name="order">The entity to modify.</param>
        /// <returns>The modified <see cref="Order"/> object or null if there was no entity.</returns>
        static public Order? RemovePassword(Order? order)
        {
            if (order is not null && order.Customer is not null)
            {
                RemovePasswords(new List<Order> { order });
            }

            return order;
        }

        /// <summary>
        /// Attempts to removes the password from all customers in a collection.
        /// </summary>
        /// <param name="customers">The collection of customers to process.</param>
        /// <returns>A <see cref="List{T}"/> collection of modified customers.</returns>
        static public List<Customer> RemovePasswords(IEnumerable<Customer> customers)
        {
            foreach (var customer in customers)
            {
                customer.Password = "";
            }

            return customers.ToList();
        }

        /// <summary>
        /// Attempts to removes the password from all orders in a collection.
        /// </summary>
        /// <param name="orders">The collection of orders to process.</param>
        /// <returns>A <see cref="List{T}"/> collection of modified orders.</returns>
        static public List<Order> RemovePasswords(IEnumerable<Order> orders)
        {
            foreach (var order in orders.Where(x => x.Customer is not null))
            {
                order.Customer.Password = "";
            }

            return orders.ToList();
        }

        #endregion
    }
}
