using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using FribergCarRentalsBravo.DataAccess.Entities.Customer;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public class CustomerRepository : ICustomer
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public Customer CreateCustomer(Customer customer)
        {
            applicationDbContext.Add(customer);
            applicationDbContext.SaveChanges();
            return customer;
        }

        public void DeleteCustomer(Customer customer)
        {
            applicationDbContext.Remove(customer);
            applicationDbContext.SaveChanges();
        }

        public Customer EditCustomer(Customer customer)
        {
            applicationDbContext.Update(customer);
            applicationDbContext.SaveChanges();
            return customer;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return applicationDbContext.Customers.OrderBy(c => c.CustomerId);
        }

        public Customer GetCustomerById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
