using FribergCarRentalsBravo;

namespace FribergCarRentalsBravo.DataAccess.Entities.Customer
{
    public interface ICustomer
    {
        Task<Customer> GetCustomerById(int id);
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> EditCustomer(Customer customer);
        Task DeleteCustomer(Customer customer);
    }
}
