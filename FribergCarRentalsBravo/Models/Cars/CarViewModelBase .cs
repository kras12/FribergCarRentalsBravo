using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Cars
{
    /// <summary>
    /// A view model base class that handles car data. 
    /// </summary>
    public abstract class CarViewModelBase : ViewModelBase
    {
        #region Constants

        /// <summary>
        /// The maximum allowed model year for a car.
        /// </summary>
        protected const int MaxCarModelYear = 2200;

        /// <summary>
        /// The minimum allowed model year for a car.
        /// </summary>
        protected const int MinCarModelYear = 1900;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        protected CarViewModelBase()
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
        protected CarViewModelBase(CarCategory category, string brand, string color, string model, int modelYear, 
            string registrationNumber, decimal rentalCostPerDay, bool isActive)
        {
            #region Checks

            if (category is null)
            {
                throw new ArgumentNullException(nameof(category), $"The value of parameter '{category}' can't be null");
            }

            if (brand is null)
            {
                throw new ArgumentNullException(nameof(brand), $"The value of parameter '{brand}' can't be null");
            }

            if (color is null)
            {
                throw new ArgumentNullException(nameof(color), $"The value of parameter '{color}' can't be null");
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), $"The value of parameter '{model}' can't be null");
            }

            if (modelYear < MinCarModelYear || modelYear > MaxCarModelYear)
            {
                throw new ArgumentOutOfRangeException(nameof(modelYear), $"The value of parameter '{modelYear}' ({modelYear}) must fit in the interval of '{MinCarModelYear}' and '{MaxCarModelYear}'.");
            }

            if (registrationNumber is null)
            {
                throw new ArgumentNullException(nameof(registrationNumber), $"The value of parameter '{registrationNumber}' can't be null");
            }

            if (RentalCostPerDay < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RentalCostPerDay), $"The value of parameter '{RentalCostPerDay}' can't be negative.");
            }

            #endregion

            Brand = brand;
            Color = color;
            Model = model;
            ModelYear = modelYear;
            RegistrationNumber = registrationNumber;
            RentalCostPerDay = rentalCostPerDay;
            IsActive = isActive;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The brand for the car.
        /// </summary>
        [DisplayName("Brand")]
        public virtual string Brand { get; set; } = "";

        /// <summary>
        /// Returns a short description of the car (brand, model, and year).
        /// </summary>
        [DisplayName("Car")]
        [BindNever]
        public string CarInfo
        {
            get
            {
                return $"{Brand} {Model} {ModelYear} ";
            }
        }

        /// <summary>
        /// Returns a short description of the car (registration number - brand, model, and year).
        /// </summary>
        [DisplayName("Car")]
        [BindNever]
        public string CarInfoWithRegistrationNumber
        {
            get
            {
                return $"{RegistrationNumber} - {Brand} {Model} {ModelYear} ";
            }
        }

        /// <summary>
        /// The color for the car.
        /// </summary>
        [DisplayName("Color")]
        public virtual string Color { get; set; } = "";

        /// <summary>
        /// The model for the car.
        /// </summary>
        [DisplayName("Model")]
        public virtual string Model { get; set; } = "";

        /// <summary>
        /// The model year for the car.
        /// </summary>
        [DisplayName("Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = MandatoryFieldValidationMessage)]
        [Range(MinCarModelYear, MaxCarModelYear)]        
        public virtual int ModelYear { get; set; }

        /// <summary>
        /// The registration number for the car.
        /// </summary>
        [DisplayName("Reg Nr")]
        public virtual string RegistrationNumber { get; set; } = "";

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost per day")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DefaultFloatNumberInputFormatString)]
        public virtual decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// Returns true if the car is active in the rental platform.
        /// </summary>
        [DisplayName("Is Active")]
        public virtual bool IsActive { get; set; }

        #endregion
    }
}