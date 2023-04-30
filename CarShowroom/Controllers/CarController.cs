
using CarShowroom.Data;
using CarShowroom.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost]
        public async Task<IActionResult> Edit(Car car)
        {
            var cars = await carShowroomContext.Cars.FindAsync(car.CarId);
            if(cars != null)
            {
                cars.CarId = car.CarId;
                cars.Model = car.Model;
                cars.Hp = car.Hp;
                cars.MaxSpeed = car.MaxSpeed;
                cars.MinSpeed = car.MinSpeed;
                cars.TypeFuel = car.TypeFuel;
                cars.Capacity = car.Capacity;
                cars.TypeEngine = car.TypeEngine;
                cars.NumberOfSeats = car.NumberOfSeats;
                cars.Height = car.Height;
                cars.Weight = car.Weight;
                cars.AverageExpenseTown = car.AverageExpenseTown;
                cars.AverageExpenseOnroad = car.AverageExpenseOnroad;
                cars.AverageExpenseCombined = car.AverageExpenseCombined;
                cars.YearOfManufacure = car.YearOfManufacure;
                cars.Doors = car.Doors;
                cars.TypeCompartment = car.TypeCompartment;
                cars.OriginalPrice = car.OriginalPrice;
                cars.PictureUrl = car.PictureUrl;
                await carShowroomContext.SaveChangesAsync();
            }

            return View("Details",car);
        }
        public async Task<IActionResult> Delete(Car car)
        {
            var cars = await carShowroomContext.Cars.FindAsync();
            if(cars != null)
            {
                carShowroomContext.Cars.Remove(car);
                await carShowroomContext.SaveChangesAsync();
            }
            return View("Index");
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
                CarId = car.CarId,
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
                PictureUrl = car.PictureUrl
            };
            await carShowroomContext.Cars.AddAsync(cars);
            await carShowroomContext.SaveChangesAsync();
            ViewBag.Message = "Car created SUCCESFULLY!";
            return View("Details");
        }

    }
}
