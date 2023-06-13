
using CarShowroom.Data;
using CarShowroom.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Xml;
using System.Xml.Linq;

namespace CarShowroom.Controllers
{
    public class CarController : Controller
    {

        private readonly CarShowroomContext carShowroomContext;
        
        public CarController(CarShowroomContext showroomContext)
        {
           
            carShowroomContext = showroomContext;
        }

        public IActionResult Index()
        {
            var cars = carShowroomContext.Cars.ToList();
            return View("Index", cars);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var cars = carShowroomContext.Cars.FirstOrDefault(x => x.CarId == id);
           
            return View("Details", cars);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cars = await carShowroomContext.Cars.FirstOrDefaultAsync(x => x.CarId == id);
            if (cars != null)
            {
                var car = new Car()
                {
                    CarId = cars.CarId,
                    Model = cars.Model,
                    Hp = cars.Hp,
                    MaxSpeed = cars.MaxSpeed,
                    MinSpeed = cars.MinSpeed,
                    TypeFuel = cars.TypeFuel,
                    Capacity = cars.Capacity,
                    TypeEngine = cars.TypeEngine,
                    NumberOfSeats = cars.NumberOfSeats,
                    Height = cars.Height,
                    Weight = cars.Weight,
                    AverageExpenseTown = cars.AverageExpenseTown,
                    AverageExpenseOnroad = cars.AverageExpenseOnroad,
                    AverageExpenseCombined = cars.AverageExpenseCombined,
                    YearOfManufacure = cars.YearOfManufacure,
                    Doors = cars.Doors,
                    TypeCompartment = cars.TypeCompartment,
                    OriginalPrice = cars.OriginalPrice,
                    PictureUrl = cars.PictureUrl

                };
                
            }

            return View("Edit", cars);

        }
        [HttpGet]
        public ActionResult DetailInformation()
        {
            var cars = carShowroomContext.Cars.Include(c => c.TestDrives).ToList();
            return View("DetailInformation",cars);
        }
        [HttpGet]
        public IActionResult GetTestDrives(int carId)
        {
            var testDrives = carShowroomContext.TestDrives.Where(td => td.CarId == carId).ToList();
            return Json(testDrives);
        }
        [HttpGet]
        public IActionResult GetOrders(int carId)
        {
            var testDrives = carShowroomContext.Orders.Where(td => td.CarId == carId).ToList();
            return Json(testDrives);
        }
        [HttpGet]
        public void ExportToExcel()
        {
            // Fetch the data from the second grid
            var testDrives = carShowroomContext.TestDrives.ToList();

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create the worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Test Drives");

                worksheet.Cells[1, 1].Value = "TestDriveID";
                worksheet.Cells[1, 2].Value = "Date of Test Drive";
                worksheet.Cells[1, 3].Value = "Date of Query";
                worksheet.Cells[1, 4].Value = "Customer ID";

                // Apply styling to the header cells
                using (var headerCells = worksheet.Cells[1, 1, 1, 4])
                {
                    headerCells.Style.Font.Bold = true;
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Set the data rows
                int row = 2;
                foreach (var testDrive in testDrives)
                {
                    worksheet.Cells[row, 1].Value = testDrive.TestDriveId;
                    worksheet.Cells[row, 2].Value = testDrive.DateOfTestDrive;
                    worksheet.Cells[row, 2].Style.Numberformat.Format = "mm/dd/yyyy hh:mm";
                    worksheet.Cells[row, 3].Value = testDrive.DateOfQuery;
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "mm/dd/yyyy hh:mm";
                    worksheet.Cells[row, 4].Value = testDrive.CustomerId;
                    row++;
                }

                // Auto-fit the columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                Response.Clear();
                // Set the content type and header for the response
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Headers.Add("content-disposition", "attachment:  filename=testDrives.xlsx");

                // Write the Excel package to the response stream
                Response.Body.WriteAsync(package.GetAsByteArray());
                
            }

            
        }
        [HttpGet]
        public IActionResult ExportToXml()
        {
            var cars = carShowroomContext.Cars.Include(c => c.TestDrives).ToList();

            // Create an XML document
            var xmlDoc = new XmlDocument();
            var rootElement = xmlDoc.CreateElement("testDrives");

            // Iterate through the cars and test drives and add them to the XML document
            foreach (var car in cars)
            {
                foreach (var testDrive in car.TestDrives)
                {
                    var testDriveElement = xmlDoc.CreateElement("testDrive");

                    var dateOfTestDriveElement = xmlDoc.CreateElement("dateOfTestDrive");
                    dateOfTestDriveElement.InnerText = testDrive.DateOfTestDrive.ToString();
                    testDriveElement.AppendChild(dateOfTestDriveElement);

                    var dateOfQueryElement = xmlDoc.CreateElement("dateOfQuery");
                    dateOfQueryElement.InnerText = testDrive.DateOfQuery.ToString();
                    testDriveElement.AppendChild(dateOfQueryElement);

                    var customerIdElement = xmlDoc.CreateElement("customerId");
                    customerIdElement.InnerText = testDrive.CustomerId.ToString();
                    testDriveElement.AppendChild(customerIdElement);

                    var carIdElement = xmlDoc.CreateElement("carId");
                    carIdElement.InnerText = testDrive.CarId.ToString();
                    testDriveElement.AppendChild(carIdElement);

                    rootElement.AppendChild(testDriveElement);
                }
            }

            xmlDoc.AppendChild(rootElement);

            // Set the response content type to "application/xml"
            Response.ContentType = "text/xml";

            // Set the file name and content disposition header for the download
            var fileName = "testDrives.xml";
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
        [HttpPost]
        public async Task<IActionResult> Edit(Car car)
        {
            var existingCar = await carShowroomContext.Cars.FindAsync(car.CarId);
            if (existingCar != null)
            {
                // Проверка на последна модификация
                if (existingCar.Modified19118133 < DateTime.Now)
                {
                    existingCar.CarId = car.CarId;
                    existingCar.Model = car.Model;
                    existingCar.Hp = car.Hp;
                    existingCar.MaxSpeed = car.MaxSpeed;
                    existingCar.MinSpeed = car.MinSpeed;
                    existingCar.TypeFuel = car.TypeFuel;
                    existingCar.Capacity = car.Capacity;
                    existingCar.TypeEngine = car.TypeEngine;
                    existingCar.NumberOfSeats = car.NumberOfSeats;
                    existingCar.Height = car.Height;
                    existingCar.Weight = car.Weight;
                    existingCar.AverageExpenseTown = car.AverageExpenseTown;
                    existingCar.AverageExpenseOnroad = car.AverageExpenseOnroad;
                    existingCar.AverageExpenseCombined = car.AverageExpenseCombined;
                    existingCar.YearOfManufacure = car.YearOfManufacure;
                    existingCar.Doors = car.Doors;
                    existingCar.TypeCompartment = car.TypeCompartment;
                    existingCar.OriginalPrice = car.OriginalPrice;
                    existingCar.PictureUrl = car.PictureUrl;
                    existingCar.Modified19118133 = DateTime.Now;

                    await carShowroomContext.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = car.CarId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Възникна конфликт. Редът е бил променен от друг потребител.";
                    return RedirectToAction("Edit", new { id = car.CarId });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Неуспешно редактиране на данните. Опитайте отново, колега/колежке.";
                return RedirectToAction("Edit", new { id = car.CarId });
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var order = await carShowroomContext.Cars.FindAsync(id);
            if (order != null)
            {
                return View("Delete", order);
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var cars = await carShowroomContext.Cars.FindAsync(id);
            if (cars != null)
            {
                carShowroomContext.Cars.Remove(cars);
                await carShowroomContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Неуспешно изтриване на данните. Опитайте отново, колега/колежке.";
                return RedirectToAction("Details", new { id = cars.CarId });
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> CreateProcess(Car car)
        {
            var cars = new Car()
            {
                
                Model = car.Model,
                Hp = car.Hp,
                MaxSpeed = car.MaxSpeed,
                MinSpeed = car.MinSpeed,
                TypeFuel = car.TypeFuel,
                Capacity = car.Capacity,
                TypeEngine = car.TypeEngine,
                NumberOfSeats = car.NumberOfSeats,
                Height = car.Height,
                Weight = car.Weight,
                AverageExpenseTown = car.AverageExpenseTown,
                AverageExpenseOnroad = car.AverageExpenseOnroad,
                AverageExpenseCombined = car.AverageExpenseCombined,
                YearOfManufacure = car.YearOfManufacure,
                Doors = car.Doors,
                TypeCompartment = car.TypeCompartment,
                OriginalPrice = car.OriginalPrice,
                PictureUrl = car.PictureUrl,
                Modified19118133 = DateTime.Now
            };
            await carShowroomContext.Cars.AddAsync(cars);
            await carShowroomContext.SaveChangesAsync();
            if (cars.CarId > 0)
            {
                
                return RedirectToAction("Details", new { id = cars.CarId });
            }
            else
            {
                TempData["ErrorMessage"] = "Колата не беше създадена и добавена в списъка с коли. Опитайте отново.";
                return RedirectToAction("Create");
            }
            
        }

    }
}
