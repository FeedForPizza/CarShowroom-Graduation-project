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
            return View("Index", td);
            
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
            var td = await carShowroomContext.TestDrives.Include(x => x.Car).Include(x => x.Customer).FirstOrDefaultAsync(x => x.TestDriveId == id);
            var cars = carShowroomContext.Cars.ToList();
            ViewBag.CustomerOptions = new SelectList(cars, "CarId", "Model");
            if (td != null)
            {
                var tds = new TestDrive()
                {
                    TestDriveId = td.TestDriveId,
                    DateOfTestDrive = td.DateOfTestDrive,
                    DateOfQuery = td.DateOfQuery,
                    CarId = td.CarId,
                    CustomerId = td.CustomerId

                };
                

            };

            return View("Edit", td);
            
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TestDrive testDrive,int selectedCarId)
        {
            
            var td = await carShowroomContext.TestDrives.Include(x => x.Car).Include(x => x.Customer).FirstOrDefaultAsync(x => x.TestDriveId == testDrive.TestDriveId);
            if (td != null)
            {
                td.TestDriveId = testDrive.TestDriveId;
                td.DateOfTestDrive = testDrive.DateOfTestDrive;
                td.DateOfQuery = DateTime.Now;
                td.CarId = selectedCarId;
                td.CustomerId = testDrive.CustomerId;
                td.Customer.FirstName = testDrive.Customer.FirstName;
                td.Customer.MiddleName = testDrive.Customer.MiddleName;
                td.Customer.LastName = testDrive.Customer.LastName;
                td.Customer.Phone = testDrive.Customer.Phone;
                td.Customer.Address = testDrive.Customer.Address;
                await carShowroomContext.SaveChangesAsync();

            }
            return View("Details");
        }
        public async Task<IActionResult> Delete(TestDrive testDrive)
        {
            var td = await carShowroomContext.TestDrives.FindAsync(testDrive.TestDriveId);
            if (td != null)
            {
                carShowroomContext.TestDrives.Remove(testDrive);
                await carShowroomContext.SaveChangesAsync();
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            var cars = carShowroomContext.Cars.ToList();
            ViewBag.CustomerOptions = new SelectList(cars, "CarId", "Model");
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> ProcessCreate(TestDrive testDrive,Customer customer)
        {
            

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
            int customerId = cust.CustomerId;
            var td = new TestDrive()
            {
                CarId = testDrive.CarId,
                DateOfTestDrive = testDrive.DateOfTestDrive,
                DateOfQuery = DateTime.Now,
                CustomerId = customerId
            };

            await carShowroomContext.TestDrives.AddAsync(td);
            await carShowroomContext.SaveChangesAsync();
           
            return View("Details");
        }
        
    }
}
