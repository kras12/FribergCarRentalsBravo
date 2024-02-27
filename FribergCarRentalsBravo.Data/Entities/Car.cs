using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Entities
{
    /// <summary>
    /// A an entity class that represents a car.
    /// </summary>
    public class Car
    {
        #region Constants

        /// <summary>
        /// The oldest model year to support. Cars have been manufactured since late 1700. 
        /// </summary>
        private const int OldestModelYearSupported = 1700;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor
        /// </summary>
        public Car()
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
        /// <param name="isActive">Returns true if the car is active in the rental program.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Car(CarCategory category, string brand, string color, string model, int modelYear, string registrationNumber,
            decimal rentalCostPerDay, bool isActive)
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
                throw new ArgumentNullException(nameof(model), "The value of parameter '{model}' can't be null");
            }

            //  We may have to support the next year's model
            if (modelYear < OldestModelYearSupported || modelYear > DateTime.Now.Year + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(modelYear), $"The model year must be between '{OldestModelYearSupported}' and '{DateTime.Now.Year + 1}'.");
            }

            if (registrationNumber is null)
            {
                throw new ArgumentNullException(nameof(registrationNumber), $"The value of parameter '{registrationNumber}' can't be null");
            }

            if (rentalCostPerDay < 0)
            {
                throw new ArgumentOutOfRangeException($"The value of parameter '{rentalCostPerDay}' can't be negative.");
            }

            #endregion

            Brand = brand;
            Color = color;
            Model = model;
            ModelYear = modelYear;
            RegistrationNumber = registrationNumber;
            Category = category;
            RentalCostPerDay = rentalCostPerDay;
            IsActive = isActive;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The brand for the car.
        /// </summary>
        public string Brand { get; set; } = "";

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [Key]
        public int CarId { get; set; }

        /// <summary>
        /// The category for the car.
        /// </summary>
        [Required]
        public CarCategory? Category { get; set; }

        /// <summary>
        /// The color for the car.
        /// </summary>
        public string Color { get; set; } = "";

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        public List<CarImage> Images { get; set; } = new();

        /// <summary>
        /// Returns true if the car is active in the rental program. 
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// The model for the car.
        /// </summary>
        public string Model { get; set; } = "";

        /// <summary>
        /// The model year for the car.
        /// </summary>
        public int ModelYear { get; set; }

        /// <summary>
        /// The registration number for the car.
        /// </summary>
        public string RegistrationNumber { get; set; } = "";

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        public decimal RentalCostPerDay { get; set; }

        #endregion
    }
}
