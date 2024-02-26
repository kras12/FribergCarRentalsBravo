using FribergCarRentalsBravo;

namespace FribergCarRentalsBravo.DataAccess.Entities.Customer
{
    public interface ICustomer
    {
        Customer GetCustomerById(int id);
        IEnumerable<Customer> GetAllCustomers();
        Customer CreateCustomer(Customer customer);
        Customer EditCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
