using demo_webshop.Data;
using demo_webshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace demo_webshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Dependency injection
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Jednostavni test za provjeru ako je sesija aktivna
            // ViewBag.CheckSession = HttpContext.Session.IsAvailable;

            // Test za kreiranje sesije
            HttpContext.Session.SetString("test", "Hej, ja sam sesija");
            // 
            ViewBag.GetTest = HttpContext.Session.GetString("test");

            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.GetTest = HttpContext.Session.GetString("test");

            return View();
        }

        // GET; /home/Products
        public IActionResult Product(int? categoryId)
        {

            //List<Product> products = _context.Products.ToList();
            List<Product> products = (categoryId != null) ?
                // ako categoryId postoji
            _context.Products.Where( // popis svih proizvoda i postavljamo kriterij
                p => _context.ProductCategories.Where(
                    pc => pc.CategoryId == categoryId  // ako je u tablici ProductCategories vrijednost stupca CategoryId = categoryId
                    ).Select(
                        pc => pc.ProductId // ako je kriterij zadobojen, vrati vrijednost stupca productId
                        ).ToList().Contains(
                            p.Id  // Nakon toga, vrati objekte klase Product, čiji ID se nalazi u rezultatu kriterija
                        )
                        ).ToList() :
                        //ako categoryId ne postoji
            _context.Products.ToList();

            

            ViewBag.Categories = _context.Categories.ToList();

            return View(products);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}