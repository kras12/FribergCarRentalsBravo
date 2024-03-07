using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBravo.Components
{
    public class TodaysPickupsListViewComponent : ViewComponent
    {
        private readonly IOrderRepository orderRep;

        public TodaysPickupsListViewComponent(IOrderRepository orderRep)
        {
            this.orderRep = orderRep;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orders = (await orderRep.GetAllTodaysPickupsAsync()).Select(x => new OrderViewModel(x)).ToList();
            return View(orders);
        }
    }
}
