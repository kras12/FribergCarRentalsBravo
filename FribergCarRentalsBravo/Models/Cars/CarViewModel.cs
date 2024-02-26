using FribergCarRentals.Models.Cars;
using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Cars
{
    /// <summary>
    /// A view model class that handles car data.
    /// </summary>
    public class CarViewModel : CarViewModelBase
    {

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="car">The car to model.</param>
        public CarViewModel(Car car)
            : base(car.Brand, car.Color, car.Model, car.ModelYear, car.RegistrationNumber, car.RentalCostPerDay, car.IsActive)
        {
            #region Checks

            if (car.Category is null)
            {
                throw new ArgumentNullException(nameof(car.Category), $"The value of property '{nameof(car.Category)}' can't be null");
            }

            if (car.CarId < 0)
            {
                throw new ArgumentNullException(nameof(car.CarId), $"The value of property '{nameof(car.CarId)}' can't be null");
            }

            if (car.Images is null)
            {
                throw new ArgumentNullException(nameof(car.Images), $"The value of property '{nameof(car.Images)}' can't be null");
            }

            #endregion

            Category = new CarCategoryViewModel(car.Category);
            CarId = car.CarId;
            Images = car.Images.Select(x => new CarImageViewModel(x)).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("ID")]
        [BindNever]
        public int CarId { get; }

        /// <summary>
        /// The category for the car.
        /// </summary>
        [DisplayName("Category")]
        public virtual CarCategoryViewModel Category { get; set; }

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        [BindNever]
        public List<CarImageViewModel> Images { get; } = new();

        /// <summary>
        /// A text that shows whether the car is active. 
        /// </summary>
        [DisplayName("Is Active")]
        public string IsActiveText
        {
            get
            {
                return IsActive ? "Active" : "Inactive";
            }
        }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost per day")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DefaultPriceOutputFormatString)]
        [BindNever]
        public override decimal RentalCostPerDay { get; set; }

        #endregion
    }
}