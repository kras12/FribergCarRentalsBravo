using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.Models.Customers;
using FribergCarRentalsBravo.Models.Cars;

namespace FribergCarRentalsBravo.Models.Orders
{
    /// <summary>
    ///  A view model class that handles data relating to an order. 
    /// </summary>
    public class OrderViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carOrder">The car order to model.</param>
        /// <param name="isNewOrder">True if the order was just created.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OrderViewModel(Order carOrder, bool isNewOrder = false)
        {
            #region Checks

            if (carOrder.Customer is null)
            {
                throw new ArgumentNullException("The customer of the order can't be null");
            }

            #endregion

            IsCancelled = carOrder.IsCanceled;
            Car = new CarViewModel(carOrder.Car);
            CarOrderId = carOrder.OrderId;
            OrderDate = carOrder.OrderDate;
            Customer = new CustomerViewModel(carOrder.Customer);
            IsNewOrder = isNewOrder;
            CarPickupDate = carOrder.PickupDate;
            CarReturnDate = carOrder.ReturnDate;
            RentalCostPerDay = carOrder.CostPerDay;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car that was rented.
        /// </summary>
        [DisplayName("Car")]
        [BindNever]
        public CarViewModel Car { get; }

        /// <summary>
        /// The ID of the order.
        /// </summary>
        [DisplayName("Order ID")]
        [BindNever]
        public int CarOrderId { get; }

        /// <summary>
        /// The car pickup date.
        /// </summary>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [BindNever]
        public DateTime CarPickupDate { get; }

        /// <summary>
        /// The car return date.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [BindNever]
        public DateTime CarReturnDate { get; }

        /// <summary>
        /// The customer that placed the order.
        /// </summary>
        [BindNever]
        public CustomerViewModel Customer { get; }

        /// <summary>
        /// Returns true if the order can be cancelled.
        /// </summary>
        [DisplayName("Is Cancelable")]
        [BindNever]
        public bool IsCancelable
        {
            get
            {
                return !IsCancelled && CarPickupDate.Date > DateTime.UtcNow.Date;
            }
        }

        /// <summary>
        /// Returns true if the order is cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Returns true if the order was just created.
        /// </summary>
        [DisplayName("New Order")]
        [BindNever]
        public bool IsNewOrder { get; private set; }

        /// <summary>
        /// The order date.
        /// </summary>
        [DisplayName("Order Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [BindNever]
        public DateTime OrderDate { get; }

        /// <summary>
        /// The total sum of the order.
        /// </summary>
        [DisplayName("Order Sum")]
        [DisplayFormat(DataFormatString = DefaultPriceOutputFormatString)]
        [BindNever]
        public decimal OrderSum
        {
            get
            {
                return RentalCostPerDay * ((CarReturnDate - CarPickupDate).Days + 1);
            }
        }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost Per Day")]
        [DisplayFormat(DataFormatString = DefaultPriceOutputFormatString)]
        [BindNever]
        public decimal RentalCostPerDay { get; }

        #endregion
    }
}
