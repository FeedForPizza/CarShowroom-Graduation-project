using CarShowroom.Data;
using CarShowroom.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CarShowroom.Controllers
{
    public class TestDriveController : Controller
    {
        private readonly CarShowroomContext carShowroomContext;
        public TestDriveController(CarShowroomContext showroomContext)
        {

            carShowroomContext = showroomContext;
        }

        public IActionResult Index()
        {
            var td = carShowroomContext.TestDrives.ToList();
            var cars = carShowroomContext.Cars.ToList();
            ViewBag.CustomerOptions = new SelectList(cars, "CarId", "Model");
            return View("Index", td);

        }
        [HttpGet]
        public ActionResult SearchMethod(int? carId, DateTime? startDate, DateTime? endDate, string sortColumn, string sortDirection)
        {
            IQueryable<TestDrive> searchResults = carShowroomContext.TestDrives;

            if (carId.HasValue)
            {
                searchResults = searchResults.Where(t => t.CarId == carId.Value);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                searchResults = searchResults.Where(t => t.DateOfTestDrive >= startDate.Value && t.DateOfTestDrive <= endDate.Value);
            }
            switch (sortColumn)
            {
                case "dateOfTestDrive":
                    if (sortDirection == "asc")
                        searchResults = searchResults.OrderBy(t => t.DateOfTestDrive);
                    else
                        searchResults = searchResults.OrderByDescending(t => t.DateOfTestDrive);
                    break;
                    // Add more cases for other columns if needed
            }
            var result = searchResults.Select(t => new
            {
                testDriveId = t.TestDriveId,
                carId = t.CarId,
                dateOfTestDrive = t.DateOfTestDrive,
                dateOfQuery = t.DateOfQuery,
                customerId = t.CustomerId
            }).ToList();

            return Json(result);
        }


        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var td = await carShowroomContext.TestDrives.Include(x => x.Car).Include(x => x.Customer).FirstOrDefaultAsync(x => x.TestDriveId == id);

            return View("Details", td);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var td = await carShowroomContext.TestDrives
    .Include(x => x.Car)
    .Include(x => x.Customer)
    .FirstOrDefaultAsync(x => x.TestDriveId == id);
            var cars = carShowroomContext.Cars.ToList();
            ViewBag.CustomerOptions = new SelectList(cars, "CarId", "Model");
            if (td != null)
            {
                // Test drive found, proceed with your logic
                var testDrive = new TestDrive()
                {
                    DateOfTestDrive = td.DateOfTestDrive,
                    DateOfQuery = td.DateOfQuery,
                    CarId = td.CarId,
                    CustomerId = td.CustomerId
                };

                // ... other code ...

                return View("Edit", td);
            }
            else
            {
                // Test drive not found, handle the scenario (e.g., display an error message or redirect to a different page)
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TestDrive testDrive)
        {
            var td = await carShowroomContext.TestDrives
    .Include(y => y.Car)
    .Include(y => y.Customer)
    .FirstOrDefaultAsync(x => x.TestDriveId == testDrive.TestDriveId);

            if (td != null)
            {
                // Проверка на последна модификация
                if (td.Modified19118133 < DateTime.Now)
                {
                    td.DateOfTestDrive = testDrive.DateOfTestDrive;
                    td.DateOfQuery = testDrive.DateOfQuery;
                    td.CarId = testDrive.CarId;
                    td.Customer.FirstName = testDrive.Customer.FirstName;
                    td.Customer.MiddleName = testDrive.Customer.MiddleName;
                    td.Customer.LastName = testDrive.Customer.LastName;
                    td.Customer.Phone = testDrive.Customer.Phone;
                    td.Customer.Address = testDrive.Customer.Address;
                    td.Modified19118133 = DateTime.Now;

                    await carShowroomContext.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = testDrive.TestDriveId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Възникна конфликт. Редът е бил променен от друг потребител.";
                    return RedirectToAction("Edit", new { id = testDrive.TestDriveId });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Неуспешно редактиране на данните. Опитайте отново, колега/колежке.";
                return RedirectToAction("Edit", new { id = testDrive.TestDriveId });
            }

        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var order = await carShowroomContext.TestDrives.Include(x => x.Car).Include(x => x.Customer).FirstOrDefaultAsync(x => x.TestDriveId == id);
            if (order != null)
            {
                return View("Delete",order);
            }
            else
            {
                return NotFound();
            }
        }
        
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var testDrive = await carShowroomContext.TestDrives.FindAsync(id);
            if (testDrive != null)
            {
                carShowroomContext.TestDrives.Remove(testDrive);
                await carShowroomContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Неуспешно изтриване на данните. Опитайте отново, колега/колежке.";
                return RedirectToAction("Details", new { id = testDrive.TestDriveId });
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            var cars = carShowroomContext.Cars.ToList();
            ViewBag.CustomerOptions = new SelectList(cars, "CarId", "Model");
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> ProcessCreate(TestDrive testDrive, Customer customer)
        {


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
            int customerId = cust.CustomerId;
            var td = new TestDrive()
            {
                CarId = testDrive.CarId,
                DateOfTestDrive = testDrive.DateOfTestDrive,
                DateOfQuery = DateTime.Now,
                CustomerId = customerId,
                Modified19118133 = DateTime.Now
            };

            await carShowroomContext.TestDrives.AddAsync(td);
            await carShowroomContext.SaveChangesAsync();
            if (td.TestDriveId > 0)
            {

                return RedirectToAction("Details", new { id = td.TestDriveId });
            }
            else
            {
                TempData["ErrorMessage"] = "Поради грешка вашият час за тест драйв не беше запазен. Моля опитайте отново.";
                return RedirectToAction("Create");
            }
            
        }

    }
}
