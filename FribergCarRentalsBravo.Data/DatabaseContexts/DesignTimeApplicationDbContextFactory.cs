using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using FribergCarRentalsBravo.Shared.SharedClasses;

namespace FribergCarRentalsBravo.DataAccess.DatabaseContexts
{
    /// <summary>
    /// A design time database context needed to support scaffolding when the database context class resides in a standalone project.
    /// </summary>
    /// <remarks>This class is needed for both MVC and Razor Pages projects. Despite the documentation stating that the class can reside in 
    /// the main project, it only seems to work when it's in the data access layer. This creates some challenges with separation of layers in the project.</remarks>
    public class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        #region Methods

        /// <summary>
        /// Creates a new instance of a derived context.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <typeparamref name="TContext" />.</returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(AppSettingsHelper.AppSettingsDevelopmentJsonFileName)
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(AppSettingsHelper.ApplicationDbContextConnectionStringKey));

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        #endregion
    }
}
