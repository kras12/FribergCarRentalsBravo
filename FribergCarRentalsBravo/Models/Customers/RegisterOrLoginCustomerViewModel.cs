namespace FribergCarRentalsBravo.Models.Customers
{
    public class RegisterOrLoginCustomerViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public RegisterOrLoginCustomerViewModel()
        {

        }

        #endregion

        #region Properties

        public RegisterCustomerViewModel RegisterCustomerViewModel { get; set; } = new();

        public LoginCustomerViewModel LoginCustomerViewModel { get; set; } = new();

        #endregion
    }
}
