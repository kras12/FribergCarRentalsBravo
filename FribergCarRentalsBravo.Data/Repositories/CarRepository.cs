using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Channels;
using System.Linq.Expressions;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    /// <summary>
    /// A repository class that handles the car entity.
    /// </summary>
    /// <remarks>This repository class works on detached entities. All fetched entities will not be tracked by EF Core.</remarks>
    public class CarRepository : ICarRepository
    {
        #region Fields

        /// <summary>
        /// The database context.
        /// </summary>
        protected readonly ApplicationDbContext _databaseContext;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        public CarRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a car to the database.
        /// </summary>
        /// <param name="car">The car to add.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public async Task AddAsync(Car car)
        {
            await _databaseContext.Cars.AddAsync(car);
            SetEnumPropertiesTrackingStateUnchanged(car);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="car">The car to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task DeleteAsync(Car car)
        {
            _databaseContext.Cars.Remove(car);
            return _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id)
        {
            var car = new Car() { CarId = id };
            _databaseContext.Cars.Remove(car);
            return _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Performs a search in the database and returns found cars.
        /// </summary>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <param name="predicate">The predicate to use.</param>
        /// <returns>A <see cref="Task"/> object containing a collection of the resulting cars.</returns>
        public async Task<IEnumerable<Car>> FindAsync(Expression<Func<Car, bool>> predicate)
        {
            return await _databaseContext.Cars.Include(x => x.Category).Include(x => x.Images).Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Gets all cars from the database.
        /// </summary>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <returns>A <see cref="Task"/> object containg a collection of all cars found.</returns>
        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _databaseContext.Cars.Include(x => x.Category).Include(x => x.Images).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Attempts to fetch a car with a specific ID.
        /// </summary>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car.</param>
        /// <remarks>This override is needed to disable tracking since the base class lacks this ability.</remarks>
        /// <returns>a <see cref="Task{TResult}"/> containing the car if found, or null if not found.</returns>
        public Task<Car?> GetByIdAsync(int id)
        {
            return _databaseContext.Cars.Include(x => x.Category).Include(x => x.Images).AsNoTracking().SingleOrDefaultAsync(x => x.CarId == id);
        }

        /// <summary>
        /// Attempts to fetch all images for a car with a specific ID. 
        /// </summary>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>s>
        /// <param name="id">The ID of the car.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a collection containing the images found.</returns>
        public async Task<IEnumerable<CarImage>> GetCarImagesAsync(int id)
        {
            // EF Core doesn't like the combination of include and AsNoTracking in this case, so we do a work around. 
            var car = await GetByIdAsync(id);
            return car?.Images ?? new List<CarImage>();
        }

        /// <summary>
        /// Returns all the cars that matches the specified category and that are available to be rented out within the desired timespan. 
        /// </summary>
        /// <param name="pickupDate">The pickup date for the car.</param>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <param name="category">The category of the car.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a collection of matching cars.</returns>
        public async Task<IEnumerable<Car>> GetRentableCarsAsync(DateTime pickupDate, CarCategory? category = null)
        {
            if (category is not null)
            {
                return await _databaseContext.Cars
                .Include(car => car.Category)
                .Include(car => car.Images)
                .GroupJoin(
                    _databaseContext.Orders,
                    car => car.CarId,
                    order => order.Car.CarId,
                    (car, orders) => new
                    {
                        car,
                        orders
                    }
                )
                .Where(orderGroup => (!orderGroup.orders.Any() || !orderGroup.orders.Any(order => order.ReturnDate >= pickupDate)) && orderGroup.car.Category == category)
                .Select(orderGroup => orderGroup.car)
                .ToListAsync();
            }
            else
            {
                return await _databaseContext.Cars
                .Include(car => car.Category)
                .Include(car => car.Images)
                .GroupJoin(
                    _databaseContext.Orders,
                    car => car.CarId,
                    order => order.Car.CarId,
                    (car, orders) => new
                    {
                        car,
                        orders
                    }
                )
                .Where(orderGroup => !orderGroup.orders.Any() || !orderGroup.orders.Any(order => order.ReturnDate >= pickupDate))
                .Select(orderGroup => orderGroup.car)
                .ToListAsync();
            }
            
        }

        /// <summary>
        /// Updates the car entity in the database. 
        /// </summary>
        /// <param name="entity">The car to update.</param>
        /// <returns>A <see cref="Task{TResult}{T}"/> object.</returns>
        public async Task UpdateAsync(Car entity)
        {
            // We must store the final images outside of the entity for our comparisions,
            // since EF Core will add tracked images to the entity if they don't exist.
            var targetImages = entity.Images.ToList();
            _databaseContext.Update(entity);
            SetEnumPropertiesTrackingStateUnchanged(entity);

            // EF Core will add missing images to the entity here.
            var databaseImages = await _databaseContext.Images.Where(x => x.Car!.CarId == entity.CarId).ToListAsync();
            
            // Modify status for deleted images
            databaseImages.ExceptBy(targetImages.Select(x => x.ImageId), y => y.ImageId).ToList()
                .ForEach(deletedImage =>
                {
                    _databaseContext.Images.Entry(deletedImage).State = EntityState.Deleted;
                });

            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Sets the necessary EF Core tracking state for enum properties in the car entity. 
        /// This is needed to instruct the framework that the status entities already exists in the database.
        /// </summary>
        /// <param name="entity">The car to set the tracking states for.</param>
        private void SetEnumPropertiesTrackingStateUnchanged(Car entity)
        {
            #region Checks

            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), $"The value of parameter '{nameof(entity)}' can't be null.");
            }

            #endregion

            // EF Core needs to know that the states already exists in the database
            _databaseContext.Entry(entity.Category!).State = EntityState.Unchanged;        
        }

        #endregion
    }
}
