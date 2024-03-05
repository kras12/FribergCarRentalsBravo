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
    public class CreateCarCategoryViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor
        public CreateCarCategoryViewModel()
        {
            
        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="categoryName">The name for the category.</param>
        public CreateCarCategoryViewModel(string categoryName)
        {
            #region Checks

            if (categoryName is null)
            {
                throw new ArgumentNullException(nameof(categoryName), $"The value of parameter '{nameof(categoryName)}' can't be null.");  
            }

            #endregion

            CategoryName = categoryName;            
        }

        #endregion

        #region Properties

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
