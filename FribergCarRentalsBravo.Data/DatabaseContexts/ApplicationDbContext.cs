using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.DatabaseContexts
{
    public class ApplicationDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        #endregion
        
        #region DBSets

        /// <summary>
        /// DBSet for cars.
        /// </summary>
        public DbSet<Car> Cars { get; set; }

        /// <summary>
        /// DBSet for car images.
        /// </summary>
        public DbSet<CarImage> Images { get; set; }

        /// <summary>
        /// DBSet for car categories.
        /// </summary>
        public DbSet<CarCategory> CarCategories { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the conventions.
        /// </summary>
        /// <param name="configurationBuilder">The configuration builder.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            // Expliclity set the precision. The default is 18,2 but we choose to give a little more precision on the decimal side. 
            // We will still be able to handle huge numbers. 
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 4);
        }

        /// <summary>
        /// Configures the options.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                optionsBuilder.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);
            }
        }
        
        /// <summary>
        /// Configures the database model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        
        #endregion
    }
}
