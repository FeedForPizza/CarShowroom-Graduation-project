using CarShowroom.Data;
using CarShowroom.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var extras = carShowroomContext.Extras.ToList();
            var options = new StringBuilder();
            foreach (var extra in extras)
            {
                options.AppendFormat("<label><input type=\"checkbox\" name=\"extraId\" value=\"{0}\" /> {1}</label><br/>", extra.ExtraId, extra.ExtraName);
            }
            ViewBag.ExtrasOptions = options.ToString();
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> CreateProcess(Order order,OrderExtra orderExtra,Extra extra,Customer customer)
        {
            var oe = new OrderExtra
            {
                OrderId = order.OrderId,
                ExtraId = extra.ExtraId
            };
            await carShowroomContext.OrderExtras.AddAsync(oe);
            await carShowroomContext.SaveChangesAsync();
            var cust = new Customer()
            {

                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                Address = customer.Address,
                Phone = customer.Phone
            };
            await carShowroomContext.Customers.AddAsync(cust);
            await carShowroomContext.SaveChangesAsync();
            int custId = cust.CustomerId;
            var orders = new Order
            {
                OrderId = order.OrderId,
                OriginalPice = order.OriginalPice,
                TotalSum = order.TotalSum,
                CarId = order.CarId,
                CustomerId = custId
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
