using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.Shared.SharedClasses
{
    /// <summary>
    /// A helper class that stores the name of the appsettings file (development) and the database connection string. 
    /// This class is used by the design database context to enable scaffolding when the database context resides in its own project.
    /// </summary>
    public static class AppSettingsHelper
    {
        #region Properties

        /// <summary>
        /// The key string in the appsettings file for the connection string.
        /// </summary>
        public static string ApplicationDbContextConnectionStringKey => "ApplicationDbContext";

        /// <summary>
        /// The file name for the development version of the appsettings file. 
        /// </summary>
        public static string AppSettingsDevelopmentJsonFileName => "appsettings.Development.json";

        #endregion
    }
}
