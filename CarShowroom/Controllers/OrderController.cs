using CarShowroom.Data;
using CarShowroom.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShowroom.Controllers
{
    public class OrderController : Controller
    {
        private readonly CarShowroomContext carShowroomContext;
        public OrderController(CarShowroomContext showroomContext)
        {

            carShowroomContext = showroomContext;
        }

        public IActionResult Index()
        {
            var orders = carShowroomContext.Orders.ToList();
            return View("Index",orders);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var order = carShowroomContext.Orders.FirstOrDefault(x => x.OrderId == id);
            return View("Details",order);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await carShowroomContext.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
            if (order != null)
            {
                var orders = new Order()
                {
                    OrderId = order.OrderId,
                    OriginalPice = order.OriginalPice,
                    TotalSum = order.TotalSum,
                    Quantity = order.Quantity,
                    CarId = order.CarId,
                    CustomerId = order.CustomerId
                };
            }
                return View("Edit",order);
        }
        public async Task<IActionResult> Edit(Order order)
        {
            var orders = await carShowroomContext.Orders.FindAsync(order.OrderId);
            if (orders != null)
            {
                orders.OrderId = order.OrderId;
                orders.OriginalPice = order.OriginalPice;
                orders.TotalSum = order.TotalSum;
                orders.Quantity = order.Quantity;
                orders.CarId = order.CarId;
                orders.CustomerId = order.CustomerId;
                await carShowroomContext.SaveChangesAsync();

            }
            return View("Details", order);
        }
        public async Task<IActionResult> Delete(Order order)
        {
            var orders = await carShowroomContext.Orders.FindAsync();
            if (orders != null)
            {
                carShowroomContext.Orders.Remove(order);
                await carShowroomContext.SaveChangesAsync();
            }
            return View("Delete");
        }
        [HttpGet]
        public ActionResult Create()
        {
            var extra = carShowroomContext.Extras.ToList();
            var viewModel = new Extra
            {
                ExtraName = 
            };
            return View("Create",extra);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProcess(Order order)
        {
            var orders = new Order
            {
                OrderId = order.OrderId,
                OriginalPice = order.OriginalPice,
                TotalSum = order.TotalSum,
                CarId = order.CarId,
                CustomerId = order.CustomerId
            };
            await carShowroomContext.Orders.AddAsync(orders);
            await carShowroomContext.SaveChangesAsync();
            ViewBag.Message = "Order made successfully!";
            return View("Details");
        }
        //public ActionResult CalculatePrice()
        //{

        //}
    }
}
