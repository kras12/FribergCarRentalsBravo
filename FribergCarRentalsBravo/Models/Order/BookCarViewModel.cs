using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.Models.Cars;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace FribergCarRentals.Models.Order
{
    /// <summary>
    /// A view model class that handles data related to order creation. 
    /// </summary>
    public class BookCarViewModel : ViewModelBase
    {
        #region Constants

        /// <summary>
        /// A constant text string to represent all car categories.
        /// </summary>
        public const string AllCarCategoriesText = "All";

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public BookCarViewModel()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="availableCarCategoryFilters">A collection of car categories that can be used as filters when searching for cars to rent.</param>
        /// <param name="havePerformedCarSearch">True if the user have performed a car search. </param>
        /// <param name="availableCars">A collection of cars that matches the chosen date filters, or all cars if no filters where chosen.</param>
        /// <param name="pickupDateFilter">The pickup date filter to use when searching for cars to rent.</param>
        /// <param name="returnDateFilter">The return date filter to use when searching for cars to rent.</param>
        /// <param name="carCategoryFilter">The car category filter to use when searching for cars to rent.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public BookCarViewModel(List<CarCategory> availableCarCategoryFilters, bool havePerformedCarSearch, List<Car>? availableCars = null, 
            DateTime? pickupDateFilter = null, DateTime? returnDateFilter = null, int? carCategoryFilter = null)
        {
            #region Checks

            if (availableCarCategoryFilters is null)
            {
                throw new ArgumentNullException(nameof(availableCars), $"The value of parameter '{availableCarCategoryFilters}' can't be null.");
            }

            #endregion

            AvailableCars = availableCars is not null ? availableCars.Select(x => new CarViewModel(x)).ToList() : new();
            AvailableCarCategoryFilters = availableCarCategoryFilters.Select(x => new CarCategoryViewModel(x)).Prepend(new CarCategoryViewModel(0, AllCarCategoriesText)).ToList();
            HavePerformedCarSearch = havePerformedCarSearch;
            PickupDateFilter = pickupDateFilter;
            ReturnDateFilter = returnDateFilter;
            SelectedCarCategoryFilter = carCategoryFilter is not null ? carCategoryFilter.Value : 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Cars available to rent 
        /// </summary>
        [BindNever]
        public List<CarViewModel> AvailableCars { get; } = new();

        /// <summary>
        /// A collection of car categories that can be used as filters when searching for cars to rent.
        /// </summary>
        [BindNever]
        public List<CarCategoryViewModel> AvailableCarCategoryFilters { get; } = new();

        /// <summary>
        /// Returns true if the user have performed a car search. 
        /// </summary>
        public bool HavePerformedCarSearch { get; }

        /// <summary>
        /// The car category filter to use when searching for cars. 
        /// </summary>
        /// <remarks>
        /// An ID of zero represents no filter. 
        /// </remarks>
        [Required]
        public int SelectedCarCategoryFilter { get; set; }

        /// <summary>
        /// The pickup date filter to use when searching for cars.
        /// </summary>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime? PickupDateFilter { get; set; } = null;

        /// <summary>
        /// The return date filter to use when searching for cars.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime? ReturnDateFilter { get; set; } = null;

        #endregion
    }
}
