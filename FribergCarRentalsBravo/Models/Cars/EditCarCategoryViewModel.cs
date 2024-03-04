using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Attributes;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Cars
{
    /// <summary>
    /// A view model class that handles data for a car category. 
    /// </summary>
    public class EditCarCategoryViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor
        public EditCarCategoryViewModel()
        {
            
        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="carCategory">The car category to model.</param>
        public EditCarCategoryViewModel(CarCategory carCategory)
        {
            #region Checks

            if (carCategory is null)
            {
                throw new ArgumentNullException(nameof(carCategory), $"The value of parameter '{nameof(carCategory)}' can't be null.");  
            }

            if (carCategory.CarCategoryId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carCategory.CarCategoryId), $"The value of property '{nameof(carCategory.CarCategoryId)}' can't be negative.");
            }

            #endregion

            CarCategoryId = carCategory.CarCategoryId;
            CategoryName = carCategory.CategoryName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car category.
        /// </summary>
        [DisplayName("Category ID")]
        [Required]
        public int CarCategoryId { get; set; }

        /// <summary>
        /// The filename for the category.
        /// </summary>
        [DisplayName("Category Name")]
        [Required]
        [StringLength(maximumLength: DefaultMaxCharacterInput, ErrorMessage = InputTooLongValidationMessage)]
        [ServerSideRegularExpression(LettersAndSpacesRegexPattern, ErrorMessage = OnlyLettersAndSpacesValidationMessage)]
        public string CategoryName { get; set; } = "";

        #endregion
    }
}
