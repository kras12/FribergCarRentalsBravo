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
        public ActionResult Index()
        {
            return View();
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            return View(customerRep.GetCustomerById(id));
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    customerRep.CreateCustomer(customer);
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
        public ActionResult Edit(int id)
        {
            if (id == null || customerRep.GetAllCustomers == null)
            {
                return NotFound();
            }

            var customer = customerRep.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customerRep.EditCustomer(customer);
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerController/Delete/5
        public ActionResult DeleteCustomer(int id)
        {
            if (id == null || customerRep.GetAllCustomers == null)
            {
                return NotFound();
            }

            var customer = customerRep.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("DeleteCustomer")] 
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCustomerConfirmed(int id)
        {
            var customer = customerRep.GetCustomerById(id);
            if (ModelState.IsValid)
            {
                try
                {
                    customerRep.DeleteCustomer(customer);
                }
                catch (Exception)
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
    }
}
