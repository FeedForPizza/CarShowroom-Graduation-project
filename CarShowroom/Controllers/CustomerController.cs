using CarShowroom.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarShowroom.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CarShowroomContext carShowroomContext;
        public CustomerController(CarShowroomContext showroomContext)
        {

            carShowroomContext = showroomContext;
        }

        public IActionResult Index()
        {
            //var products = _carService.GetProducts();
            return View("Index");
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
