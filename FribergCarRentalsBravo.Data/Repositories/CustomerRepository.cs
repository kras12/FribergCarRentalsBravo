using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<Customer> CreateCustomer(Customer customer)
        {
            applicationDbContext.Add(customer);
            await applicationDbContext.SaveChangesAsync();
            return customer;
        }

        public Task<bool> CustomerExists(string email)
        {
            return applicationDbContext.Customers.AnyAsync(x => x.Email == email);
        }

        public async Task DeleteCustomer(Customer customer)
        {
            applicationDbContext.Remove(customer);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<Customer> EditCustomer(Customer customer)
        {
            applicationDbContext.Update(customer);
            await applicationDbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await applicationDbContext.Customers.OrderBy(x => x.LastName).ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await applicationDbContext.Customers.FirstOrDefaultAsync(s => s.CustomerId == id);
        }

        public async Task<Customer?> GetMatchingCustomerAsync(string email, string password)
        {
            var customer = await applicationDbContext.Customers.AsNoTracking().Where(x => x.Email == email && x.Password == password).SingleOrDefaultAsync();

            if (customer is not null)
            {
                customer.Password = "";
                return customer;
            }

            return null;
        }

        public async Task<int> GetAmountOfCustomersAsync()
        {
            return applicationDbContext.Customers.Count();
        }

    }
}
