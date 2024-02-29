using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentalsBravo.Helpers
{
    /// <summary>
    /// A helper class to help transfer data from one object to another. 
    /// </summary>
    internal static class DataTransferHelper
    {
        #region Methods

        /// <summary>
        /// Transfer data from the source object to the destination object by copying data between matching properties. 
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TDest">The type of the destination object.</typeparam>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="destinationObject">The destination object having the copied data.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="System.Reflection.TargetException"></exception>
        /// <exception cref="MethodAccessException"></exception>
        /// <exception cref="System.Reflection.TargetInvocationException"></exception>
        /// <returns>True if data was transfered between any matching properties.</returns>
        public static bool TryTransferData<TSource, TDest>(TSource sourceObject, [NotNullWhen(returnValue: true)] out TDest destinationObject)
            where TSource : class where TDest : class, new()
        {
            destinationObject = new TDest();
            return TryTransferData(sourceObject, destinationObject);
        }

        /// <summary>
        /// Transfer data from the source object to the destination object by copying data between matching properties. 
        /// </summary>
        /// <typeparam name="TSource">The type of the source object.</typeparam>
        /// <typeparam name="TDest">The type of the destination object.</typeparam>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="destinationObject">The destination object having the copied data.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="System.Reflection.TargetException"></exception>
        /// <exception cref="MethodAccessException"></exception>
        /// <exception cref="System.Reflection.TargetInvocationException"></exception>
        /// <returns>True if data was transfered between any matching properties.</returns>
        public static bool TryTransferData<TSource, TDest>(TSource sourceObject, TDest destinationObject)
            where TSource : class where TDest : class, new()
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties();
            bool dataWasWritten = false;

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name && x.PropertyType == sourceProperty.PropertyType && x.CanWrite);

                if (destinationProperty != null)
                {
                    destinationProperty.SetValue(destinationObject, sourceProperty.GetValue(sourceObject));
                    dataWasWritten = true;
                }
            }

            return dataWasWritten;
        }

        #endregion
    }
}
