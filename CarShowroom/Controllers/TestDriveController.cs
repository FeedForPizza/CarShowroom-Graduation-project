﻿using CarShowroom.Data;
using CarShowroom.Entities;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Details(int id)
        {
            var td = carShowroomContext.TestDrives.FirstOrDefault(x => x.TestDriveId == id);
            return View("Details", td);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int custid)
        {
            var td = await carShowroomContext.TestDrives.FirstOrDefaultAsync(x => x.TestDriveId == id);
            var cust = await carShowroomContext.Customers.FirstOrDefaultAsync(x => x.CustomerId == custid);
            if (td != null)
            {
                var tds = new TestDrive()
                {
                   TestDriveId = td.TestDriveId,
                   CarId = td.CarId,
                   DateOfTestDrive = td.DateOfTestDrive,
                   DateOfQuery = td.DateOfQuery
                };
                var custom = new Customer()
                {
                    FirstName = cust.FirstName,
                    MiddleName = cust.MiddleName,
                    LastName = cust.LastName,
                    Address = cust.Address,
                    Phone = cust.Phone
                };

            };

            return View("Edit", td);
            
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TestDrive testDrive, Customer customer)
        {
            var cars = carShowroomContext.Cars.ToList();
            var options = new StringBuilder();
            foreach (var car in cars)
            {
                options.AppendFormat("<option value=\"{0}\">{1}</option>", car.CarId, car.Model);
            }
            ViewBag.CustomerOptions = options.ToString();
            var td = await carShowroomContext.TestDrives.FindAsync(testDrive.TestDriveId);
            var cust = await carShowroomContext.Customers.FindAsync(customer.CustomerId);
            if(td != null)
            {
                td.TestDriveId = testDrive.TestDriveId;
                td.CarId = testDrive.CarId;
                td.DateOfTestDrive = testDrive.DateOfTestDrive;
                td.DateOfQuery = testDrive.DateOfQuery;
                td.CustomerId = customer.CustomerId;
                cust.FirstName = customer.FirstName;    
                cust.MiddleName = customer.MiddleName;
                cust.LastName = customer.LastName;
                cust.Address = customer.Address;
                cust.Phone = customer.Phone;

            }
            return View("Details");
        }
        public async Task<IActionResult> Delete(TestDrive testDrive)
        {
            var td = await carShowroomContext.TestDrives.FindAsync();
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
            var options = new StringBuilder();
            foreach (var car in cars)
            {
                options.AppendFormat("<option value=\"{0}\">{1}</option>", car.CarId,car.Model);
            }
            ViewBag.CustomerOptions = options.ToString();
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
                TestDriveId = testDrive.TestDriveId,
                CarId = testDrive.CarId,
                DateOfTestDrive = testDrive.DateOfTestDrive,
                DateOfQuery = testDrive.DateOfQuery,
                CustomerId = customerId
            };
            
            await carShowroomContext.TestDrives.AddAsync(td);
            await carShowroomContext.SaveChangesAsync();
            ViewBag.Message = "TestDrive created SUCCESFULLY!";
            return View("Details");
        }
    }
}