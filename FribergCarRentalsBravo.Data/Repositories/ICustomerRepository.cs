using FribergCarRentalsBravo.DataAccess.Entities;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerById(int id);
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> EditCustomer(Customer customer);
        Task DeleteCustomer(Customer customer);
    }
}
