using CarShowroom.Data;
using Microsoft.AspNetCore.Mvc;

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
            var storageItems = carShowroomContext.Storages.ToList();
            return View("Index",storageItems);
        }
        public ActionResult Details()
        {
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
