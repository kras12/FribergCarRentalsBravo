using FribergCarRentalsBravo.DataAccess.Entities;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomer(Customer customer);

        public Task<bool> CustomerExists(string email);

        Task DeleteCustomer(Customer customer);

        Task DeleteCustomerByIdAsync(int id);

        Task<Customer> EditCustomer(Customer customer);

        Task<List<Customer>> GetAllCustomers();

        Task<Customer> GetCustomerById(int id);

        public Task<Customer?> GetMatchingCustomerAsync(string email, string password);
    }
}
