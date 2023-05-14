using CarShowroom.Data;
using CarShowroom.Entities;
using CarShowroom.Models;
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
            return View("Index", orders);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var order = carShowroomContext.Orders.FirstOrDefault(x => x.OrderId == id);
            return View("Details", order);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int custid)
        {
            var order = await carShowroomContext.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
            var custoomer = await carShowroomContext.Customers.FirstOrDefaultAsync(y => y.CustomerId == custid);
            if (order != null)
            {

                var orders = new Order()
                {
                    OrderId = order.OrderId,
                    OriginalPrice = order.OriginalPrice,
                    TotalSum = order.TotalSum,
                    Quantity = order.Quantity,
                    CarId = order.CarId,

                };

            }
            return View("Edit", order);
        }
        public async Task<IActionResult> Edit(Order order)
        {
            var orders = await carShowroomContext.Orders.FindAsync(order.OrderId);
            if (orders != null)
            {
                orders.OrderId = order.OrderId;
                orders.OriginalPrice = order.OriginalPrice;
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
            ViewBag.ExtrasOptions = extras.Select(x => new ExtraViewModel() { ExtraId = x.ExtraId, IsChecked = false, ExtraName = x.ExtraName });
            var cars = carShowroomContext.Cars.ToList();
            ViewBag.CustomerOptions = new SelectList(cars,"CarId","Model");
            return View("Create");
        }
        [HttpPost]
        public IActionResult CalculatePrice(int carId, int quantity, List<ExtraViewModel> extra)
        {
            var car = carShowroomContext.Cars.SingleOrDefault(c => c.CarId == carId);
            if (car == null)
            {
                return Json(new { errorMessage = "Invalid car ID" });
            }
            var selectedExtras = extra.Where(e => e.IsChecked).ToList();
            decimal extraPrice = 0;
            foreach(var e in selectedExtras)
            {
                if (e.IsChecked)
                {
                    var extraInDb = carShowroomContext.Extras.FirstOrDefault(x => x.ExtraId == e.ExtraId);
                    if (extraInDb != null)
                    {
                        extraPrice += extraInDb.Price ?? 0;
                    }
                }
            }
            // Calculate the price based on the selected car and quantity

            var price = car.OriginalPrice * quantity + extraPrice;
            

            return Json(new { price });
        }
        [HttpGet]
        public IActionResult GetCarPrice(int carId)
        {
            var car = carShowroomContext.Cars.SingleOrDefault(c => c.CarId == carId);
            if (car == null)
            {
                return Json(new { errorMessage = "Invalid car ID" });
            }

            return Json(new { ogPrice = car.OriginalPrice });
        }
        [HttpPost]
        public async Task<IActionResult> CreateProcess(Order order, List<ExtraViewModel> extras, Customer customer)
        {

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
                
                OriginalPrice = order.OriginalPrice,
                TotalSum = order.TotalSum,
                CarId = order.CarId,
                CustomerId = custId
            };
            await carShowroomContext.Orders.AddAsync(orders);
            await carShowroomContext.SaveChangesAsync();
            foreach (var extra in extras.Where(x => x.IsChecked))
            {
                var oe = new OrderExtra
                {
                    OrderId = orders.OrderId,
                    ExtraId = extra.ExtraId
                };
                await carShowroomContext.OrderExtras.AddAsync(oe);
            }
            await carShowroomContext.SaveChangesAsync();
            ViewBag.Message = "Order made successfully!";
            return View("Details");
        }
        //public ActionResult CalculatePrice()
        //{

        //}
    }
}
