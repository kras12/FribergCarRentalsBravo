using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace FribergCarRentals.Models.Orders
{
    /// <summary>
    /// A view model class that handles data related to order creation. 
    /// </summary>
    public class EditOrderViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public EditOrderViewModel()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="order">The order to model.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EditOrderViewModel(Order order)
        {
            #region Checks

            if (order is null)
            {
                throw new ArgumentNullException(nameof(order), $"The value of parameter '{nameof(order)}' can't be null.");
            }

            #endregion

            OrderId = order.OrderId;
            PickupDate = order.PickupDate;
            ReturnDate = order.ReturnDate;
            OrderDate = order.OrderDate.Date;
            CostPerDay = order.CostPerDay;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The order date.
        /// </summary>
        [DisplayName("Order Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime? OrderDate { get; set; } = null;

        /// <summary>
        /// The ID of the car for the order.
        /// </summary>
        [DisplayName("Order ID")]
        [Required]
        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }

        /// <summary>
        /// The pickup date for the car.
        /// </summary>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required(AllowEmptyStrings = false, ErrorMessage = MandatoryFieldValidationMessage)]
        public DateTime? PickupDate { get; set; } = null;

        /// <summary>
        /// The rental cost per day for the car.
        /// </summary>
        [DisplayName("Cost Per Day")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DefaultFloatNumberInputFormatString)]
        [Required]
        [Range(0, 20_000)]
        public virtual decimal CostPerDay { get; set; }

        /// <summary>
        /// The return date for the car.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required(AllowEmptyStrings = false, ErrorMessage = MandatoryFieldValidationMessage)]
        public DateTime? ReturnDate { get; set; } = null;

        #endregion
    }
}
