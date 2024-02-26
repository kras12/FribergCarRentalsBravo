using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Cars
{
    /// <summary>
    ///  A view model class that handles data for creating a new car.
    /// </summary>
    public class CreateCarViewModel : CarViewModelBase
    {

        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        public CreateCarViewModel() : base()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="category">The category for the car.</param>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="color">The color for the car.</param>
        /// <param name="model">The model for the car.</param>
        /// <param name="modelYear">The model year for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        /// <param name="rentalCostPerDay">The rental cost per day.</param>
        /// <param name="isActive">True if the car is active in the rental platform.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CreateCarViewModel(CarCategory category, string brand, string color, string model, int modelYear, 
            string registrationNumber, decimal rentalCostPerDay, bool isActive) 
            : base(category, brand, color, model, modelYear, registrationNumber, rentalCostPerDay, isActive)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The images to upload
        /// </summary>
        [DisplayName("Upload Images")]
        public List<IFormFile>? UploadImages { get; set; } = null;

        #endregion
    }
}