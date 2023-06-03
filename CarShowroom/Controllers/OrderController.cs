using CarShowroom.Data;
using CarShowroom.Entities;
using CarShowroom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CarShowroom.Controllers
{
    public class OrderController : Controller
    {
        private readonly CarShowroomContext carShowroomContext;
        public OrderController(CarShowroomContext showroomContext)
        {

            carShowroomContext = showroomContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var orders = carShowroomContext.Orders.ToList();
            return View("Index", orders);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            
            var order = await carShowroomContext.Orders.Include(x => x.Car).Include(x=>x.Customer).FirstOrDefaultAsync(x => x.OrderId == id);
            
            return View("Details", order);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await carShowroomContext.Orders.Include(x=>x.Car).Include(x=>x.Customer).FirstOrDefaultAsync(x => x.OrderId == id);
            var cars = carShowroomContext.Cars.ToList();
            ViewBag.CustomerOptions = new SelectList(cars, "CarId", "Model");
            if (order != null)
            {

                var orders = new Order()
                {
                    OrderId = order.OrderId,
                    OriginalPrice = order.OriginalPrice,
                    TotalSum = order.TotalSum,
                    Quantity = order.Quantity,
                    CarId = order.CarId,
                    CustomerId = order.CustomerId
                };

            }
            return View("Edit", order);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Order order, int selectedCarId)
        {
            var orders = await carShowroomContext.Orders.Include(x=>x.Car).Include(x=>x.Customer).FirstOrDefaultAsync(x=> x.OrderId == order.OrderId);
            if (orders != null)
            {
                orders.OrderId = order.OrderId;
                orders.OriginalPrice = order.OriginalPrice;
                orders.TotalSum = order.TotalSum;
                orders.Quantity = order.Quantity;
                orders.CarId = selectedCarId;
                orders.CustomerId = order.CustomerId;
                orders.Customer.FirstName = order.Customer.FirstName;
                orders.Customer.MiddleName = order.Customer.MiddleName;
                orders.Customer.LastName = order.Customer.LastName;
                orders.Customer.Phone = order.Customer.Phone;
                orders.Customer.Address = order.Customer.Address;
                await carShowroomContext.SaveChangesAsync();

            }
            return View("Details", orders);
        }
        public async Task<IActionResult> Delete(Order order)
        {
            var orders = await carShowroomContext.Orders.FindAsync(order.OrderId);
            if (orders != null)
            {
                carShowroomContext.Orders.Remove(order);
                await carShowroomContext.SaveChangesAsync();
            }
            return View("Index");
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
        public async Task<IActionResult> CreateProcess(Order order, List<ExtraViewModel> extras, Customer customer, string email,decimal oriPrice)
        {

            //await carShowroomContext.SaveChangesAsync();
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
                
                OriginalPrice = oriPrice,
                Quantity = order.Quantity,
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
            return RedirectToAction("CreateCardPayment", new { totalSum = order.TotalSum, email });
            //return RedirectToAction("ProcessPayment",new { email });
        }
        public IActionResult CreateCardPayment(decimal totalSum, string email)
        {
            ViewBag.Email = email;
            ViewBag.TotalSum = totalSum;
            return View("CardPayment");
        }
        [HttpGet]
        public IActionResult ProcessPayment(string email)
        {
            // Process the payment and perform any necessary operations

            // Send confirmation email using SendGrid
            var apiKey = "apiKey";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("krisito_dt@abv.bg", "Krisi");
            var subject = "Payment Confirmation";
            var to = new EmailAddress(email);
            var plainTextContent = "Thank you for your payment!";
            var htmlContent = "<p>Thank you for your payment!</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg).Result;

            return RedirectToAction("PaymentConfirmation");
        }
        public IActionResult PaymentConfirmation()
        {
            return View("Details");
        }
        public IActionResult GenerateOrderReport(string carModel, int? customerId, string sortOrder)
        {
            // Get the orders from the database based on the given criteria
            var query = carShowroomContext.Orders
                .Include(o => o.Car)
                .Include(o => o.Customer)
                .AsQueryable();

            // Apply filtering criteria
            if (!string.IsNullOrEmpty(carModel))
            {
                query = query.Where(o => o.Car.Model == carModel);
            }

            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId.Value);
            }

            // Apply sorting criteria
            switch (sortOrder)
            {
                case "model_asc":
                    query = query.OrderBy(o => o.Car.Model);
                    break;
                case "model_desc":
                    query = query.OrderByDescending(o => o.Car.Model);
                    break;
                case "total_sum_asc":
                    query = query.OrderBy(o => o.TotalSum);
                    break;
                case "total_sum_desc":
                    query = query.OrderByDescending(o => o.TotalSum);
                    break;
                // Add more sorting options as needed
                default:
                    query = query.OrderBy(o => o.OrderId);
                    break;
            }

            // Perform the query and retrieve the data
            var orders = query.ToList();

            // Map the orders to the OrderReportModel
            var reportData = orders.Select(o => new OrderReportModel
            {
                OrderId = o.OrderId,
                OriginalPrice = o.OriginalPrice,
                TotalSum = o.TotalSum,
                Quantity = o.Quantity,
                CarModel = o.Car?.Model,
                CustomerName = o.Customer.FirstName +" "+ o.Customer.MiddleName + " " + o.Customer.LastName
                // Map additional aggregated metrics properties
            });

            // Return the report data to the view
            return View(reportData);
        }
    }
}
