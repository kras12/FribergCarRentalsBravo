using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace FribergCarRentals.Models.Order
{
    /// <summary>
    /// A view model class that handles data related to order creation. 
    /// </summary>
    public class CreateOrderViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CreateOrderViewModel()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="customerId">The ID of the customer for the order.</param>
        /// <param name="car">The car for the order.</param>
        /// <param name="pickupDate">The pickup date for the car booking.</param>
        /// <param name="returnDate">The return date for the car booking.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CreateOrderViewModel(int customerId, Car car, DateTime pickupDate, DateTime returnDate)
        {
            #region Checks

            if (customerId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(customerId), $"The value of parameter '{customerId}' can't be negative.");
            }

            if (car is null)
            {
                throw new ArgumentNullException(nameof(car), $"The value of parameter '{car}' can't be null.");
            }

            #endregion

            CustomerId = customerId;
            CarId = car.CarId;
            CarDescription = $"{car.Brand} {car.Model} {car.ModelYear}";
            PickupDate = pickupDate;
            ReturnDate = returnDate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Description of the car.
        /// </summary>
        [BindNever]
        public string CarDescription { get; } = "";

        /// <summary>
        /// The ID of the car for the order.
        /// </summary>
        [DisplayName("Car ID")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive number larger than 1.")]
        public int CarId { get; set; }

        /// <summary>
        /// The ID of the customer for the order.
        /// </summary>
        [DisplayName("Customer ID")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive number larger than 1.")]
        public int CustomerId { get; set; }

        /// <summary>
        /// The pickup date for the car.
        /// </summary>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime PickupDate { get; set; }

        /// <summary>
        /// The return date for the car.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime ReturnDate { get; set; }

        #endregion
    }
}
