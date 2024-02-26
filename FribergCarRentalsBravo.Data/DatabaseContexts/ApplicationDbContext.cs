using FribergCarRentalsBravo.DataAccess.Entities.Customer;
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
        public DbSet<Customer> Customers { get; set; }

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
        public DbSet<CarCategory> CarCategories { get; set; }        

        #endregion

        #region Methods

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
