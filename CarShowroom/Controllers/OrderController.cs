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
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;

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
            var orders = carShowroomContext.Orders.Include(o=>o.Car).Include(o=>o.Customer).ToList();
            var cars = carShowroomContext.Cars.ToList();
            ViewBag.CustomerOptions = new SelectList(cars, "CarId", "Model");
            return View("Index", orders);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var order = await carShowroomContext.Orders.Include(x => x.Car).Include(x => x.Customer).FirstOrDefaultAsync(x=>x.OrderId == id);
            if (order != null)
            {
                return View("Delete", order);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {

            var order = await carShowroomContext.Orders.Include(x => x.Car).Include(x => x.Customer).FirstOrDefaultAsync(x => x.OrderId == id);

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
        public async Task<IActionResult> Edit(Order order)
        {
            var orders = await carShowroomContext.Orders
     .Include(x => x.Car)
     .Include(x => x.Customer)
     .FirstOrDefaultAsync(x => x.OrderId == order.OrderId);

            if (orders != null)
            {
                // Проверка на последна модификация
                if (orders.Modified19118133 < DateTime.Now)
                {
                    orders.OriginalPrice = order.OriginalPrice;
                    orders.TotalSum = order.TotalSum;
                    orders.Quantity = order.Quantity;
                    orders.CarId = order.CarId;

                    orders.Customer.FirstName = order.Customer.FirstName;
                    orders.Customer.MiddleName = order.Customer.MiddleName;
                    orders.Customer.LastName = order.Customer.LastName;
                    orders.Customer.Phone = order.Customer.Phone;
                    orders.Customer.Address = order.Customer.Address;
                    orders.Modified19118133 = DateTime.Now;

                    await carShowroomContext.SaveChangesAsync();

                    return RedirectToAction("Details", new { id = order.OrderId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Възникна конфликт. Редът е бил променен от друг потребител.";
                    return RedirectToAction("Edit", new { id = order.OrderId });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Редът не съществува. Опитайте отново.";
                return RedirectToAction("Edit", new { id = order.OrderId });
            }

        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await carShowroomContext.Orders.FindAsync(id);
            if (orders != null)
            {
                carShowroomContext.Orders.Remove(orders);
                await carShowroomContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Неуспешно изтриване на данните. Опитайте отново, колега/колежке.";
                return RedirectToAction("Details", new { id = orders.OrderId });
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            var extras = carShowroomContext.Extras.ToList();
            ViewBag.ExtrasOptions = extras.Select(x => new ExtraViewModel() { ExtraId = x.ExtraId, IsChecked = false, ExtraName = x.ExtraName +" "+ x.Price });
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
        public async Task<IActionResult> CreateProcess(Order order, List<ExtraViewModel> extras, Customer customer, string email,decimal originalPrice)
        {
            
            //await carShowroomContext.SaveChangesAsync();
            var cust = new Customer()
            {

                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                Address = customer.Address,
                Phone = customer.Phone,
                Modified19118133 = DateTime.Now
            };
            await carShowroomContext.Customers.AddAsync(cust);
            await carShowroomContext.SaveChangesAsync();
            int custId = cust.CustomerId;
            var orders = new Order
            {
                
                OriginalPrice = originalPrice,
                Quantity = order.Quantity,
                TotalSum = order.TotalSum,
                CarId = order.CarId,
                CustomerId = custId,
                Modified19118133 = DateTime.Now
            };
            await carShowroomContext.Orders.AddAsync(orders);
            await carShowroomContext.SaveChangesAsync();
            foreach (var extra in extras.Where(x => x.IsChecked))
            {
                var oe = new OrderExtra
                {
                    OrderId = orders.OrderId,
                    ExtraId = extra.ExtraId,
                    Modified19118133 = DateTime.Now
                };
                await carShowroomContext.OrderExtras.AddAsync(oe);
            }
            await carShowroomContext.SaveChangesAsync();

            if (orders.OrderId > 0)
            {
                TempData["OrderId"] = orders.OrderId;
                return RedirectToAction("CreateCardPayment", new { totalSum = order.TotalSum, email });
            }
            else
            {
                TempData["ErrorMessage"] = "Поради грешка вашата поръчка не беше създадена. Моля опитайте отново.";
                return RedirectToAction("Create");
            }
            
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
            var apiKey = "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";
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
        public async Task<IActionResult> PaymentConfirmation()
        {
            Order orderDetails = null;
            if (TempData.ContainsKey("OrderId"))
            {
                int orderId = (int)TempData["OrderId"];
                orderDetails = await carShowroomContext.Orders.Include(x=>x.Customer).Include(x=>x.Car).FirstOrDefaultAsync(x=> x.OrderId == orderId);
            }
            
            return View("Details", orderDetails);
        }
        [HttpGet]
        public IActionResult GenerateOrderReport(int? carModel, string sortOrder)
        {
            // Get the orders from the database based on the given criteria
            IQueryable<Order> query = carShowroomContext.Orders.Include(o=>o.Car);

            // Apply filtering criteria
            if (carModel.HasValue)
            {
                query = query.Where(o => o.CarId == carModel.Value);
            }

            // Apply sorting criteria
            switch (sortOrder)
            {
                
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

            var orders = query.ToList();

            // Map the orders to the OrderReportModel
            var reportData = orders.Select(o => new
            {
                orderId = o.OrderId,
                originalPrice = o.OriginalPrice,
                totalSum = o.TotalSum,
                quantity = o.Quantity,
                carId = o.CarId,
                model = o.Car.Model, // Include the car model
                customerId = o.CustomerId
            }).ToList();

            // Return the report data as JSON
            return Json(reportData);
        }
        [HttpGet]
        public IActionResult ExportToXml(string report)
        {
            try
            {
                
                // Deserialize the report JSON data
                var reportData = JsonConvert.DeserializeObject<List<Order>>(report);
                
                // Create the XML document and populate it with the report data
                var xmlDoc = new XmlDocument();
                
                var rootElement = xmlDoc.CreateElement("report");
                // Iterate over the list of Order objects
                foreach (var order in reportData)
                {
                    // Create an XML element for each Order object
                    var orderElement = xmlDoc.CreateElement("order");

                    var orderIdElement = xmlDoc.CreateElement("orderId");
                    orderIdElement.InnerText = order.OrderId.ToString();
                    orderElement.AppendChild(orderIdElement);

                    var originalPriceElement = xmlDoc.CreateElement("originalPrice");
                    originalPriceElement.InnerText = order.OriginalPrice.ToString();
                    orderElement.AppendChild(originalPriceElement);

                    var totalSumElement = xmlDoc.CreateElement("totalSum");
                    totalSumElement.InnerText = order.TotalSum.ToString();
                    orderElement.AppendChild(totalSumElement);

                    var quantityElement = xmlDoc.CreateElement("quantity");
                    quantityElement.InnerText = order.Quantity.ToString();
                    orderElement.AppendChild(quantityElement);

                    var carIdElement = xmlDoc.CreateElement("carId");
                    carIdElement.InnerText = order.CarId.ToString();
                    orderElement.AppendChild(carIdElement);

                    var car = GetCarById(order.CarId);
                    var carModel = xmlDoc.CreateElement("carModel");
                    carModel.InnerText = car.Model;
                    orderElement.AppendChild(carModel);

                    var customerIdElement = xmlDoc.CreateElement("customerId");
                    customerIdElement.InnerText = order.CustomerId.ToString();
                    orderElement.AppendChild(customerIdElement);

                    var customer = GetCustomerById(order.CustomerId);
                    if (customer != null)
                    {
                        var firstNameElement = xmlDoc.CreateElement("firstName");
                        firstNameElement.InnerText = customer.FirstName;
                        orderElement.AppendChild(firstNameElement);

                        var lastNameElement = xmlDoc.CreateElement("lastName");
                        lastNameElement.InnerText = customer.LastName;
                        orderElement.AppendChild(lastNameElement);

                        var middleNameElement = xmlDoc.CreateElement("middleName");
                        middleNameElement.InnerText = customer.MiddleName;
                        orderElement.AppendChild(middleNameElement);
                    }


                    // Append the order element to the root element
                    rootElement.AppendChild(orderElement);
                }

                // Append the root element to the XML document
                xmlDoc.AppendChild(rootElement);

                // Set the response content type to "application/xml"
                Response.ContentType = "text/xml";

                // Set the file name and content disposition header for the download
                var fileName = "report.xml";
                Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileName);

                // Write the XML document to the response stream
                using (var stream = new MemoryStream())
                {
                    xmlDoc.Save(stream);
                    stream.Position = 0;
                    stream.CopyTo(Response.Body);
                }

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during processing
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        private Customer GetCustomerById(int customerId)
        {
         
                var customer = carShowroomContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);
                return customer;
            
        }
        private Car GetCarById(int? carId)
        {

            var car = carShowroomContext.Cars.FirstOrDefault(c => c.CarId == carId);
            return car;

        }
    }
}
