using CarShowroom.Data;
using CarShowroom.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShowroom.Controllers
{
    public class ExtraController : Controller
    {
        private readonly CarShowroomContext carShowroomContext;
        public ExtraController(CarShowroomContext showroomContext)
        {

            carShowroomContext = showroomContext;
        }

        public IActionResult Index()
        {
            var extras = carShowroomContext.Extras.ToList();
            return View("Index",extras);
        }
        public IActionResult IndexCarExtra()
        {
            var extras = carShowroomContext.OrderExtras.Include(x=>x.Extra).ToList();
            return View("IndexCarExtra", extras);
        }
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var td = await carShowroomContext.Extras.FirstOrDefaultAsync(x => x.ExtraId == id);
            return View("Details",td);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var td = await carShowroomContext.Extras.FirstOrDefaultAsync(x => x.ExtraId == id);
            if (td != null)
            {
                var tds = new Extra()
                {
                   ExtraId = td.ExtraId,
                   ExtraName = td.ExtraName,
                   Price = td.Price

                };


            };
            return View("Edit",td);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Extra extras)
        {
            var td = await carShowroomContext.Extras.FirstOrDefaultAsync(x => x.ExtraId == extras.ExtraId);
            if (td != null)
            {
                // Проверка на последна модификация
                if (td.Modified19118133 < DateTime.Now)
                {
                    td.ExtraId = extras.ExtraId;
                    td.ExtraName = extras.ExtraName;
                    td.Price = extras.Price;
                    td.Modified19118133 = DateTime.Now;

                    await carShowroomContext.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = extras.ExtraId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Възникна конфликт. Редът е бил променен от друг потребител.";
                    return RedirectToAction("Edit", new { id = extras.ExtraId });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Неуспешно редактиране на данните. Опитайте отново, колега/колежке.";
                return RedirectToAction("Edit", new { id = extras.ExtraId });
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var order = await carShowroomContext.Extras.FindAsync(id);
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
            var extra = await carShowroomContext.Extras.FindAsync(id);
            if (extra != null)
            {
                carShowroomContext.Extras.Remove(extra);
                await carShowroomContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Неуспешно изтриване на данните. Опитайте отново, колега/колежке.";
                return RedirectToAction("Details", new { id = extra.ExtraId });
            }
        }
        public ActionResult Create()
        {
            return View("Create");
        }
        public async Task<IActionResult> ProcessCreate(Extra extra)
        {
            var cust = new Extra()
            {
                ExtraName = extra.ExtraName,
                Price = extra.Price,
                Modified19118133 = DateTime.Now

            };
            await carShowroomContext.Extras.AddAsync(cust);
            await carShowroomContext.SaveChangesAsync();
            if (cust.ExtraId > 0)
            {

                return RedirectToAction("Details", new { id = cust.ExtraId });
            }
            else
            {
                TempData["ErrorMessage"] = "Екстрата не беше създадена и добавена в списъка с екстри. Опитайте отново. ";
                return RedirectToAction("Create");
            }
            
        }
    }
}
