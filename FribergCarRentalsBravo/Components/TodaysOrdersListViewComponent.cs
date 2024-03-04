using FribergCarRentalsBravo.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBravo.Components
{
    public class TodaysOrdersListViewComponent : ViewComponent
    {
        private readonly IOrderRepository orderRep;

        public TodaysOrdersListViewComponent(IOrderRepository orderRep)
        {
            this.orderRep = orderRep;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orders = await orderRep.GetAllTodaysOrdersAsync();
            return View(orders);
        }
    }
}
