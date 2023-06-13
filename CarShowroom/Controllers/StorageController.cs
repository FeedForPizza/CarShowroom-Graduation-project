using CarShowroom.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShowroom.Controllers
{
    public class StorageController : Controller
    {
        private readonly CarShowroomContext carShowroomContext;
        public StorageController(CarShowroomContext showroomContext)
        {

            carShowroomContext = showroomContext;
        }

        public IActionResult Index()
        {
            var storageItems = carShowroomContext.Storages.Include(y=>y.Car).ToList();
            return View("Index",storageItems);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var cars = carShowroomContext.Storages.FirstOrDefault(x => x.StorageId == id);
            return View("Details");
        }
        public ActionResult Edit()
        {
            return View("Edit");
        }
        public ActionResult Delete()
        {
            return View("Delete");
        }
        public ActionResult Create()
        {
            return View("Create");
        }
    }
}
