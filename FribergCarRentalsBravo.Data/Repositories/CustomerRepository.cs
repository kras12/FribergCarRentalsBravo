using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Helpers;
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
            return PasswordHelper.RemovePassword(customer)!;
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

        public Task DeleteCustomerByIdAsync(int id)
        {
            var customer = new Customer() { CustomerId = id };
            applicationDbContext.Customers.Remove(customer);
            return applicationDbContext.SaveChangesAsync();
        }

        public async Task<Customer> EditCustomer(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Password))
            {
                customer.Password = await applicationDbContext.Customers.Where(x => x.CustomerId == customer.CustomerId).Select(x => x.Password).SingleAsync();
            }

            applicationDbContext.Update(customer);
            await applicationDbContext.SaveChangesAsync();
            return PasswordHelper.RemovePassword(customer)!;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return PasswordHelper.RemovePasswords(await applicationDbContext.Customers.ToListAsync());
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            return PasswordHelper.RemovePassword(await applicationDbContext.Customers.FirstOrDefaultAsync(s => s.CustomerId == id));
        }

        public async Task<Customer?> GetMatchingCustomerAsync(string email, string password)
        {
            return PasswordHelper.RemovePassword(await applicationDbContext.Customers.AsNoTracking().Where(x => x.Email == email && x.Password == password).SingleOrDefaultAsync());
        }

        public async Task<int> GetAmountOfCustomersAsync()
        {
            return await applicationDbContext.Customers.CountAsync();
        }        
    }
}
