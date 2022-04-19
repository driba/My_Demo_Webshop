using demo_webshop.Data;
using demo_webshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace demo_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int id)
        {
            var category = _context.Categories.Where(s => s.Id == id).FirstOrDefault();

            // var category = _context.Categories.FirstOrDefault(s => s.Id == id);

            if(category == null)
            {
                return NotFound();
                // Alternativno
                // return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int id)
        {

            var category = _context.Categories.FirstOrDefault(s => s.Id == id);

            if(category == null)
            {
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            try
            {

                _context.Categories.Update(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int id)
        {

            if(id == 0)
            {
                return RedirectToAction("Index");
            }

            var category = _context.Categories.FirstOrDefault(s => s.Id == id);

            if(category == null)
            {
                return RedirectToAction("Index");
            }


            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // Zadatak - pronađi i izbriši zapis iz tablice baze podataka
                var category = _context.Categories.FirstOrDefault(c => c.Id == id);

                if(category == null)
                {
                    return RedirectToAction("Index");
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
