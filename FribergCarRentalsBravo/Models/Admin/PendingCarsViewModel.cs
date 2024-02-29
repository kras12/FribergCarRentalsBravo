using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.Models.Cars;
using FribergCarRentalsBravo.Models.Orders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace FribergCarRentalsBravo.Models.Admin
{
    /// <summary>
    /// A view model class that handles data related to order creation. 
    /// </summary>
    public class PendingCarsViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public PendingCarsViewModel()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="havePerformedCarSearch">True if the user have performed a car search. </param>
        /// <param name="orders">A collection of orders having pickup dates that matches the chosen date filters.</param>
        /// <param name="startDateFilter">The start date to use when searching for cars.</param>
        /// <param name="endDateFilter">The end date to use when searching for cars.</param>
        public PendingCarsViewModel(bool havePerformedCarSearch, List<Order>? orders = null,
            DateTime? startDateFilter = null, DateTime? endDateFilter = null)
        {
            Orders = orders != null ? orders.Select(x => new OrderViewModel(x)).ToList() : new();
            HavePerformedCarSearch = havePerformedCarSearch;
            StartDateFilter = startDateFilter;
            EndDateFilter = endDateFilter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if the user have performed a car search. 
        /// </summary>
        public bool HavePerformedCarSearch { get; }

        /// <summary>
        /// The start date to use when searching for cars.
        /// </summary>
        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime? StartDateFilter { get; set; } = null;

        /// <summary>
        /// The end date to use when searching for cars.
        /// </summary>
        [DisplayName("End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime? EndDateFilter { get; set; } = null;

        /// <summary>
        /// A collection of orders having pickup dates that matches the chosen date filters.
        /// </summary>
        public List<OrderViewModel> Orders { get; set; } = new();

        #endregion
    }
}
