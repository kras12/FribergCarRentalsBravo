using FribergCarRentalsBravo.DataAccess.Entities.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBravo.Controllers
{
    public class CustomerController : Controller
    {
        public ICustomer customerRep { get; }

        public CustomerController(ICustomer customerRep)
        {
            this.customerRep = customerRep;            
        }

        // GET: CustomerController
        public async Task<ActionResult> Index()
        {
            var customers = await customerRep.GetAllCustomers();
            return View(customers);
        }

        // GET: CustomerController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View(await customerRep.GetCustomerById(id));
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await customerRep.CreateCustomer(customer);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || customerRep.GetAllCustomers == null)
            {
                return NotFound();
            }

            Customer customer = await customerRep.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await customerRep.EditCustomer(customer);
                }
                catch (Exception)
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: CustomerController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Customer customer = await customerRep.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: CustomerController/Delete/5             

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await customerRep.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }

            try
            {
                await customerRep.DeleteCustomer(customer);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Det gick inte att ta bort kunden. Försök igen senare.");
                return View(customer);
            }
        }
    }
}