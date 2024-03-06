using FribergCarRentalsBravo.DataAccess.Repositories;
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
            var orders = await orderRep.GetAllTodaysPickupsAsync();
            return View(orders);
        }
    }
}
