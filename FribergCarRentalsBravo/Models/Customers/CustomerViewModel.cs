﻿using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Models.Users;

namespace FribergCarRentalsBravo.Models.Customers
{
    /// <summary>
    /// A view model class that handles data for a customer.
    /// </summary>
    public class CustomerViewModel : UserViewModelBase
    {
        #region Constructors

        public CustomerViewModel()
        {
            
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customer">The customer to copy data from.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CustomerViewModel(Customer customer) 
            : base(customer.FirstName, customer.LastName, customer.Email)
        {
            CustomerId = customer.CustomerId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the customer.
        /// </summary>
        [DisplayName("Customer ID")]
        [BindNever]
        public int CustomerId { get; set; }

        #endregion
    }
}
