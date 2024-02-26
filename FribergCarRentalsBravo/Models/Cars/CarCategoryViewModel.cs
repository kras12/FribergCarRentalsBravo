using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;

namespace FribergCarRentalsBravo.Models.Cars
{
    /// <summary>
    /// A view model class that handles data for a car category. 
    /// </summary>
    public class CarCategoryViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="category">The car category to model.</param>
        public CarCategoryViewModel(CarCategory category)
            : this(category.CarCategoryId, category.Name)
        {

        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="categoryId">The ID for the category.</param>
        /// <param name="categoryName">The name for the category.</param>
        public CarCategoryViewModel(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;            
        }

        #endregion

        #region Properties

        /// <summary>
        /// The filename for the category.
        /// </summary>
        [DisplayName("Category")]
        [BindNever]
        public string CategoryName { get; } = "";

        /// <summary>
        /// The ID for the category.
        /// </summary>
        [DisplayName("Category ID")]
        [BindNever]
        public int? CategoryId { get; }

        #endregion
    }
}
