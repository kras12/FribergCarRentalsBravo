using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Entities
{
    public class Order
    {
        #region Constructors


        public Order()
        {
            
        }

        public Order(DateTime orderDate, Car car, Customer customer, DateTime pickupDate, DateTime returnDate, decimal costPerDay)
        {
            OrderDate = orderDate;
            Car = car;
            Customer = customer;
            PickupDate = pickupDate;
            ReturnDate = returnDate;
            CostPerDay = costPerDay;
        }

        #endregion

        #region Properties

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Car Car { get; set; }
        public Customer Customer { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal CostPerDay { get; set; }
        public bool IsCanceled { get; set; }

        #endregion
    }
}
